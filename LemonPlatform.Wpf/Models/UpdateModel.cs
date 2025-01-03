namespace LemonPlatform.Wpf.Models
{
    public class UpdateModel
    {
        public string Version { get; set; }

        public string CurrentVersion { get; set; }

        public string Description { get; set; }

        public bool HasNewVersion { get; set; } = false;

        public List<UpdateFileInfo> Main { get; set; } = new List<UpdateFileInfo>();

        public List<UpdateFileInfo> Modules { get; set; } = new List<UpdateFileInfo>();
    }

    public class UpdateFileInfo
    {
        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public long FileSize { get; set; }
    }
}