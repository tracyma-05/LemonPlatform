using MaterialDesignThemes.Wpf;

namespace LemonPlatform.Wpf.Configs
{
    internal class ThemeConfig
    {
        public bool IsDarkTheme { get; set; }
        public bool IsColorAdjusted { get; set; }
        public float DesiredContrastRatio { get; set; }
        public Contrast ContrastValue { get; set; }
        public ColorSelection ColorSelectionValue { get; set; }
    }
}