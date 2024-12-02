using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Media;

namespace LemonPlatform.Wpf.Helpers
{
    internal class ThemeHelper
    {
        public static void SetLemonTheme(bool isDark = false)
        {
            SystemThemeHelper.DwmGetColorizationColor(out int pcrColorization, out _);
            var color = SystemThemeHelper.GetMediaColor(pcrColorization);
            var secondaryColor = SystemParameters.WindowGlassColor;

            SetPrimaryColorAndBaseTheme(color, secondaryColor, isDark);
        }

        private static void SetPrimaryColorAndBaseTheme(Color primaryColor, Color secondaryColor, bool isDark = false)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            var baseTheme = isDark ? BaseTheme.Dark : BaseTheme.Light;
            theme.SetBaseTheme(baseTheme);
            theme.SetPrimaryColor(primaryColor);
            theme.SetSecondaryColor(secondaryColor);
            paletteHelper.SetTheme(theme);
        }

        internal static void SetPrimaryColor()
        {
            SystemThemeHelper.DwmGetColorizationColor(out int pcrColorization, out _);
            var color = SystemThemeHelper.GetMediaColor(pcrColorization);
            var secondaryColor = SystemParameters.WindowGlassColor;

            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            theme.SetPrimaryColor(color);
            theme.SetSecondaryColor(secondaryColor);
            paletteHelper.SetTheme(theme);
        }

        internal static void ModifyTheme(Action<Theme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}