using LemonPlatform.Core;
using LemonPlatform.Core.Infrastructures.Dependency;
using LemonPlatform.Core.Infrastructures.Ioc;
using LemonPlatform.SQLite;
using LemonPlatform.Wpf.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
            services.AddDbContextServices();

            IocManager.Instance.ServiceProvider = services.BuildServiceProvider();
        }

        private static void AddDbContextServices(this IServiceCollection services)
        {
            services.AddDbContext<LemonDbContext>(options =>
            {
                var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lemon.db");
                options.UseSqlite($"Data Source={databasePath}")
                    .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            });
        }

        private static void AddAssemblyServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(WpfModule))!;
            var coreAssembly = Assembly.GetAssembly(typeof(ILemonModule))!;

            services.AddServiceAssembly(assembly);
            services.AddServiceAssembly(coreAssembly);
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
                var modules = CreateLemonModule(assembly);

                services.AddServiceAssembly(assembly);
                foreach (var item in modules)
                {
                    item.RegisterServices(services);
                    LemonConstants.PageItems.AddRange(item.GetMenuItems());
                }
            }
        }

        private static Assembly LoadPlugin(string path)
        {
            //var loadContext = new PluginLoadContext(path);
            //return loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(path));

            return Assembly.LoadFrom(path);
        }

        private static IEnumerable<ILemonModule> CreateLemonModule(Assembly assembly)
        {
            var lemonModules = new List<ILemonModule>();
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(ILemonModule).IsAssignableFrom(type))
                {
                    var result = Activator.CreateInstance(type) as ILemonModule;
                    if (result != null)
                    {
                        lemonModules.Add(result);
                    }
                }
            }

            return lemonModules;
        }
    }
}