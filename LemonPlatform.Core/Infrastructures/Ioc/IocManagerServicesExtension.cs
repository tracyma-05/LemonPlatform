using Microsoft.Extensions.DependencyInjection;

namespace LemonPlatform.Core.Infrastructures.Ioc
{
    public static class IocManagerServicesExtension
    {
        public static void AddNextIocManager(this IServiceCollection services)
        {
            services.AddSingleton<IIocManager, IocManager>(provider =>
            {
                IocManager.Instance.ServiceProvider = provider;
                return IocManager.Instance;
            });
        }
    }
}