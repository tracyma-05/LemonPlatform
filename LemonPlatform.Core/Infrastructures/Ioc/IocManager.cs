namespace LemonPlatform.Core.Infrastructures.Ioc
{
    public class IocManager : IIocManager
    {
        static IocManager()
        {
            Instance = new IocManager();
        }

        public static IocManager Instance { get; private set; }
        public IServiceProvider ServiceProvider { get; set; }
    }
}