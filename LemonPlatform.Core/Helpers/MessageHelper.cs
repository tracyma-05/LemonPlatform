using CommunityToolkit.Mvvm.Messaging;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Models;

namespace LemonPlatform.Core.Helpers
{
    public class MessageHelper
    {
        public static void SendLemonMessage(MessageType messageType, object? context)
        {
            WeakReferenceMessenger.Default.Send(new LemonMessage
            {
                MessageType = messageType,
                Content = context
            });
        }

        public static void SendBusyMessage(BusyItem context)
        {
            SendLemonMessage(MessageType.IsBusy, context);
        }

        public static void SendMenuMessage(string context)
        {
            SendLemonMessage(MessageType.Menu, context);   
        }

        public static void SendPluginMessage(PluginItem context)
        {
            SendLemonMessage(MessageType.Plugin, context);
        }

        public static void SendSnackMessage(string context)
        {
            SendLemonMessage(MessageType.Snack, context);
        }

        public static void SendStatusBarTextMessage(string context)
        {
            SendLemonMessage(MessageType.StatusBarText, context);
        }

        public static void SendStatusBarProcessMessage(bool context)
        {
            SendLemonMessage(MessageType.StatusBarProcess, context);
        }
    }
}