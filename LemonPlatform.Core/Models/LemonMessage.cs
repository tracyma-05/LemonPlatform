using LemonPlatform.Core.Enums;

namespace LemonPlatform.Core.Models
{
    public class LemonMessage
    {
        public MessageType MessageType { get; set; }

        public object? Content { get; set; }
    }
}