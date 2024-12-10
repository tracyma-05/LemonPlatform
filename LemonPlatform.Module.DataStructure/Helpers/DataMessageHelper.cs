using CommunityToolkit.Mvvm.Messaging;
using LemonPlatform.Module.DataStructure.Messages;

namespace LemonPlatform.Module.DataStructure.Helpers
{
    internal class DataMessageHelper
    {
        internal static void SendMessage(RenderType type, string message)
        {
            WeakReferenceMessenger.Default.Send(new RenderMessage
            {
                Type = type,
                Message = message
            });
        }
    }
}