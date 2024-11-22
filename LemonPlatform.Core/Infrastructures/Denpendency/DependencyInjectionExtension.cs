using LemonPlatform.Core.Infrastructures.Denpendency;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LemonPlatform.Core.Infrastructures.Dependency
{
    public static class DependencyInjectionExtension
    {
        public static void AddServiceAssembly(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies == null | assemblies?.Length == 0)
            {
                throw new Exception("assemblies cannot be empty.");
            }

            foreach (var assembly in assemblies!)
            {
                RegisterDependenciesByAssembly<ISingletonDependency>(services, assembly);
                RegisterDependenciesByAssembly<ITransientDependency>(services, assembly);
                RegisterDependenciesByAssembly<IScopeDependency>(services, assembly);
            }
        }

        public static void RegisterDependenciesByAssembly<TServiceLifetime>(IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes().Where(x => typeof(TServiceLifetime).GetTypeInfo().IsAssignableFrom(x) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract && !x.GetTypeInfo().IsSealed).ToList();
            foreach (var type in types)
            {
                var serviceRegisterAttribute = type.GetCustomAttribute<ServiceRegisterAttribute>();
                if (serviceRegisterAttribute != null)
                {
                    var lifetime = serviceRegisterAttribute.Lifetime;
                    var registerType = serviceRegisterAttribute.RegisterType;
                    switch (lifetime)
                    {
                        case Lifetime.TransientDependency:
                            services.AddKeyedTransient(registerType, serviceRegisterAttribute.RegisterName ?? KeyedService.AnyKey, type);
                            break;
                        case Lifetime.ScopeDependency:
                            services.AddKeyedScoped(registerType, serviceRegisterAttribute.RegisterName ?? KeyedService.AnyKey, type);
                            break;
                        case Lifetime.SingletonDependency:
                            services.AddKeyedSingleton(registerType, serviceRegisterAttribute.RegisterName ?? KeyedService.AnyKey, type);
                            break;
                    }

                    continue;
                }

                var iType = type.GetTypeInfo().GetInterfaces().FirstOrDefault(x => x.Name.ToUpper().Contains(type.Name.ToUpper())) ?? type;
                var serviceLifetime = FindServiceLifetime(typeof(TServiceLifetime));
                services.Add(new ServiceDescriptor(iType, type, serviceLifetime));
            }
        }

        private static ServiceLifetime FindServiceLifetime(Type type)
        {
            if (type == typeof(ISingletonDependency))
            {
                return ServiceLifetime.Singleton;
            }
            if (type == typeof(ITransientDependency))
            {
                return ServiceLifetime.Transient;
            }
            if (type == typeof(IScopeDependency))
            {
                return ServiceLifetime.Scoped;
            }

            throw new ArgumentOutOfRangeException($"Provided ServiceLifetime type is invalid. Lifetime:{type.Name}");
        }
    }
}