using LemonPlatform.Core.Models;

namespace LemonPlatform.Core.Commons
{
    public class LemonConstants
    {
        public static List<PluginItem> PageItems = new List<PluginItem>();

        public const string ApplicationName = "LemonPlatform";

        public const string DbName = "lemon.db";

        public static List<ILemonModule> Modules = new List<ILemonModule>();
    }
}