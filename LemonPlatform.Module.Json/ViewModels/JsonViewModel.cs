using CommunityToolkit.Mvvm.ComponentModel;
using LemonPlatform.Core.Exceptions;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Infrastructures.Ioc;
using LemonPlatform.Module.Json.Renders;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LemonPlatform.Module.Json.ViewModels
{
    [ObservableObject]
    public partial class JsonViewModel : ITransientDependency
    {
        public JsonViewModel()
        {
            InitDependencyData();
        }

        [ObservableProperty]
        private int _width;

        [ObservableProperty]
        private int _height;

        [ObservableProperty]
        private ITreeRender _render;

        private void InitDependencyData()
        {
            Height = 625;
            Width = 941;

            var types = GetRenderTypes();

            Render = GetKeyedService<ITreeRender>("SkipListRender");

            Render.Width = Width;
            Render.Height = Height;
        }

        private T GetKeyedService<T>(string name)
        {
            var service = IocManager.Instance.ServiceProvider.GetKeyedService<T>(name);
            if (service == null) throw new LemonException($"Can not find {name} service.");
            return service;
        }


        private List<string> GetRenderTypes()
        {
            var result = new List<string>();
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => typeof(ITreeRender).GetTypeInfo().IsAssignableFrom(x)
                          && x.GetTypeInfo().IsClass
                          && !x.GetTypeInfo().IsAbstract);

            foreach (var item in types)
            {
                result.Add(item.Name);
            }

            return result;
        }
    }
}