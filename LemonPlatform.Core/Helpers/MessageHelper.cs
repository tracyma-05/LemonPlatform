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
    }
}