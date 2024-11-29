using LemonPlatform.Core;
using LemonPlatform.Core.Commons;
using LemonPlatform.Core.Infrastructures.Dependency;
using LemonPlatform.Core.Infrastructures.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace LemonPlatform.Wpf
{
    internal static class WpfModule
    {
        internal static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddAssemblyServices();
            services.AddNextIocManager();
            services.AddModuleServices();
            services.AddCoreServices(context.Configuration);

            IocManager.Instance.ServiceProvider = services.BuildServiceProvider();
        }

        private static void AddAssemblyServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(WpfModule))!;
            services.AddServiceAssembly(assembly);
        }

        private static void AddModuleServices(this IServiceCollection services)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modules");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return;
            }

            LemonConstants.PageItems.Clear();
            foreach (var dir in Directory.GetDirectories(path))
            {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (!File.Exists(pluginDll))
                {
                    continue;
                }

                var assembly = LoadPlugin(pluginDll);
                services.AddServiceAssembly(assembly);

                CreateLemonModule(assembly);
            }

            foreach (var item in LemonConstants.Modules)
            {
                LemonConstants.PageItems.AddRange(item.GetMenuItems());
            }
        }

        private static Assembly LoadPlugin(string path)
        {
            //var loadContext = new PluginLoadContext(path);
            //return loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(path));

            return Assembly.LoadFrom(path);
        }

        private static void CreateLemonModule(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(ILemonModule).IsAssignableFrom(type))
                {
                    var result = Activator.CreateInstance(type) as ILemonModule;
                    if (result != null)
                    {
                        LemonConstants.Modules.Add(result);
                    }
                }
            }
        }
    }
}