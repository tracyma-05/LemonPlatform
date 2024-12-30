using Flurl.Http;
using LemonPlatform.Wpf.Models;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Windows;

namespace LemonPlatform.Wpf.Helpers
{
    internal class UpdateHelper
    {
        private const string RepositoryOwner = "tracyma-05";
        private const string RepositoryName = "LemonPlatform";
        private const string MainPrefix = "LemonPlatform.Main";
        private const string ModulePrefix = "LemonPlatform.Module";

        public static async Task<UpdateModel> CheckForUpdatesAsync()
        {
            var result = new UpdateModel();
            var currentVersion = GetCurrentVersion();
            var url = $"https://api.github.com/repos/{RepositoryOwner}/{RepositoryName}/releases/latest";
            var response = await url.WithHeader("User-Agent", "WPF-App-Updater").GetStringAsync();
            var release = JsonDocument.Parse(response);
            result.Version = release.RootElement.GetProperty("tag_name").GetString()!;
            if (!IsNewVersionAvailable(currentVersion, result.Version)) return result;
            var asserts = release.RootElement.GetProperty("assets");
            foreach (var item in asserts.EnumerateArray())
            {
                var name = item.GetProperty("name").ToString();
                var downloadUrl = item.GetProperty("browser_download_url").ToString();
                if (name.Contains(MainPrefix))
                {
                    result.Main.Add(name, downloadUrl);
                }

                if (name.Contains(ModulePrefix))
                {
                    result.Modules.Add(name, downloadUrl);
                }
            }

            return result;
        }

        private static string GetCurrentVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return version.ToString();
        }

        private static bool IsNewVersionAvailable(string currentVersion, string latestVersion)
        {
            return new Version(latestVersion).CompareTo(new Version(currentVersion)) > 0;
        }

        public static async Task DownloadAndUpdateAsync(string downloadUrl)
        {
            var tempFilePath = Path.GetTempFileName();
            using (var client = new HttpClient())
            {
                var data = await client.GetByteArrayAsync(downloadUrl);
                await File.WriteAllBytesAsync(tempFilePath, data);
            }

            var result = MessageBox.Show("新版本可用。是否更新？", "更新", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Process.Start(tempFilePath);
                Application.Current.Shutdown();
            }
        }
    }
}