using LemonPlatform.Core.Models;

namespace LemonPlatform.Core.Commons
{
    public class LemonConstants
    {
        public static List<PluginItem> PageItems = new List<PluginItem>();
        public static List<PluginItem> ChatItems = new List<PluginItem>();
        public static PluginItem? SelectChatItem = null;

        public const string ApplicationName = "LemonPlatform";

        public const string DbName = "lemon.db";

        public const string RootDialog = "RootDialog";

        public static List<ILemonModule> Modules = new List<ILemonModule>();

        public const string GuestUserId = "Guest";
        public const string ThemeConfigId = "ThemeConfig";
        public const string ChatConfigId = "ChatConfig";
    }
}