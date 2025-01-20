namespace LemonPlatform.CustomControls.Controls.TreeViews.Models
{
    public class FileItem
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string Type { get; set; }

        public string Extension { get; set; }

        public long? Size { get; set; }

        public int Depth { get; set; }

        public List<FileItem> Children { get; set; }
    }
}