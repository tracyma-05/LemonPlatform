namespace LemonPlatform.Core.Helpers
{
    public class EnumHelper
    {
        public static string[] GetEnumNames<T>() where T : Enum
        {
            return Enum.GetNames(typeof(T));
        }
    }
}