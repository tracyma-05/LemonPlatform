namespace LemonPlatform.Core.Infrastructures.Denpendency
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceRegisterAttribute : Attribute
    {
        public ServiceRegisterAttribute(Type registerType, Lifetime lifetime)
        {
            RegisterType = registerType;
            Lifetime = lifetime;
        }

        public ServiceRegisterAttribute(Type registerType, Lifetime lifetime, string registerName)
            : this(registerType, lifetime)
        {
            RegisterName = registerName;
        }

        public Type RegisterType { get; set; }

        public Lifetime Lifetime { get; set; }

        public string? RegisterName { get; set; }
    }
}