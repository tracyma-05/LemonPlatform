namespace LemonPlatform.Core.Infrastructures.Ioc
{
    public interface IIocManager
    {
        IServiceProvider ServiceProvider { get; set; }
    }
}