using System.Text.Json.Serialization;

namespace LemonPlatform.Wpf.Models
{
    public class ZipModel
    {
        public string Source { get; set; }

        public string Target { get; set; }

        public int Order { get; set; }
    }
}