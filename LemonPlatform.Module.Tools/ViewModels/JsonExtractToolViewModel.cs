using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Exceptions;
using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Text.Json.Nodes;

namespace LemonPlatform.Module.Tools.ViewModels
{
    [ObservableObject]
    public partial class JsonExtractToolViewModel : ITransientDependency
    {
        [NotifyCanExecuteChangedFor(nameof(ParseCommand))]
        [NotifyCanExecuteChangedFor(nameof(ExtractCommand))]
        [ObservableProperty]
        private string _input;

        [ObservableProperty]
        private string _parseItem;

        [ObservableProperty]
        private string _response;

        [NotifyCanExecuteChangedFor(nameof(ExtractCommand))]
        [ObservableProperty]
        private string _key;

        [RelayCommand(CanExecute = nameof(ParseCanExecute))]
        private void Parse()
        {
            if (TryParse(Input, out var node) && node is JsonArray or JsonObject)
            {
                var result = IterateNode(node, null);
                ParseItem = string.Join("\r\n", result.Item1.Distinct().ToList());
            }
            else
            {
                throw new LemonException("not json format");
            }
        }

        [RelayCommand(CanExecute = nameof(ExtractCanExecute))]
        private void Extract()
        {
            if (TryParse(Input, out var node) && node is JsonArray or JsonObject)
            {
                var result = IterateNode(node, Key.Trim());
                Response = string.Join("\r\n", result.Item2.Distinct().ToList());
            }
            else
            {
                throw new LemonException("not json format");
            }
        }

        private bool ParseCanExecute()
        {
            return !string.IsNullOrWhiteSpace(Input);
        }

        private bool ExtractCanExecute()
        {
            return !string.IsNullOrWhiteSpace(Input) && !string.IsNullOrEmpty(Key);
        }

        private (List<string>, List<string>) IterateJsonObject(JsonObject o, string baseKey = null, string extractKey = null)
        {
            var result = new List<string>();
            var valResult = new List<string>();
            var keys = new List<string>(o.Count);
            keys.AddRange(o.Select(sub => sub.Key));
            foreach (var key in keys)
            {
                var subKey = string.IsNullOrEmpty(baseKey) ? key : $"{baseKey}:{key}";
                switch (o[key])
                {
                    case JsonObject obj:
                        var objectResult = IterateJsonObject(obj, subKey, extractKey);
                        result.AddRange(objectResult.Item1);
                        valResult.AddRange(objectResult.Item2);
                        break;
                    case JsonArray array:
                        var arrayResult = IterateJsonArray(array, subKey, extractKey);
                        result.AddRange(arrayResult.Item1);
                        valResult.AddRange(arrayResult.Item2);
                        break;
                    case JsonValue val:
                        if (TryParse(val.ToString(), out var innerNode) && innerNode is JsonObject or JsonArray)
                        {
                            IterateNode(innerNode, extractKey);
                            o[key] = innerNode.ToString();
                        }
                        else
                        {
                            result.Add(subKey);
                            if (string.CompareOrdinal(subKey, extractKey) == 0)
                            {
                                valResult.Add(val.ToString());
                            }
                        }

                        break;
                }
            }

            return (result, valResult);
        }

        private (List<string>, List<string>) IterateJsonArray(JsonArray arr, string baseKey = null, string extractKey = null)
        {
            var result = new List<string>();
            var valResult = new List<string>();
            for (var i = 0; i < arr.Count; i++)
            {
                switch (arr[i])
                {
                    case JsonObject obj:
                        var objectResult = IterateJsonObject(obj, baseKey, extractKey);
                        result.AddRange(objectResult.Item1);
                        valResult.AddRange(objectResult.Item2);
                        break;
                    case JsonArray array:
                        var arrayResult = IterateJsonArray(array, baseKey, extractKey);
                        result.AddRange(arrayResult.Item1);
                        valResult.AddRange(arrayResult.Item2);
                        break;
                    case JsonValue val:
                        if (TryParse(val.ToString(), out var innerNode) && innerNode is JsonObject or JsonArray)
                        {
                            IterateNode(innerNode, extractKey);
                            arr[i] = innerNode.ToString();
                        }
                        else
                        {
                            result.Add(baseKey);
                            if (string.CompareOrdinal(baseKey, extractKey) == 0)
                            {
                                valResult.Add(val.ToString());
                            }
                        }

                        break;
                }
            }

            return (result, valResult);
        }

        private (List<string>, List<string>) IterateNode(JsonNode node, string key)
        {
            switch (node)
            {
                case JsonObject obj:
                    return IterateJsonObject(obj, null, key);
                case JsonArray arr:
                    return IterateJsonArray(arr, null, key);
            }

            return (new List<string>(), new List<string>());
        }

        private bool TryParse(string json, out JsonNode node)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                node = default;
                return false;
            }
            if (!((json.StartsWith('[') && json.EndsWith(']')) || (json.StartsWith('{') && json.EndsWith('}'))))
            {
                node = default;
                return false;
            }
            try
            {
                node = JsonNode.Parse(json);
                return true;
            }
            catch
            {
                node = default;
                return false;
            }
        }
    }
}