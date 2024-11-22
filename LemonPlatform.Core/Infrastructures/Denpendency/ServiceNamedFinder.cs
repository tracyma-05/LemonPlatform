using Microsoft.Extensions.DependencyInjection;

namespace LemonPlatform.Core.Infrastructures.Denpendency
{
    public class ServiceNamedFinder
    {
        public ServiceLifetime LifeTime { get; set; } = ServiceLifetime.Transient;

        public Func<Type, bool>? ServiceType { get; set; }
    }
}