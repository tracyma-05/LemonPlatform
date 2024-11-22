namespace LemonPlatform.Core.Models
{
    [Flags]
    public enum UserRole
    {
        SuperAdmin = 1,
        Administrator = 2,
        NormalUser = 4,
        Guest = 8,
    }
}