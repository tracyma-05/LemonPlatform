namespace LemonPlatform.Wpf.Models
{
    public class UpdateTask
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public long FileSize { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }

        public bool IsMain { get; set; }
    }
}