namespace LemonPlatform.Wpf.Models
{
    public class UpdateModel
    {
        public string Version { get; set; }

        public string CurrentVersion { get; set; }

        public string Description { get; set; }

        public bool HasNewVersion { get; set; } = false;

        public Dictionary<string, string> Main { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> Modules { get; set; } = new Dictionary<string, string>();
    }
}