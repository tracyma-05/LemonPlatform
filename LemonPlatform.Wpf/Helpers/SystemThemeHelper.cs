using System.Runtime.InteropServices;
using System.Windows.Media;
using Microsoft.Win32;

namespace LemonPlatform.Wpf.Helpers
{
    public static class SystemThemeHelper
    {
        // See "https://docs.microsoft.com/en-us/windows/win32/api/dwmapi/nf-dwmapi-dwmgetcolorizationcolor"
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmGetColorizationColor(out int pcrColorization, out bool pfOpaqueBlend);

        public static System.Drawing.Color GetDrawingColor(int argb) => System.Drawing.Color.FromArgb(argb);

        public static Color GetMediaColor(int argb) => new()
        {
            A = (byte)(argb >> 24),
            R = (byte)(argb >> 16),
            G = (byte)(argb >> 8),
            B = (byte)(argb)
        };

        // true为深色模式 反之false
        public static bool GetWindowsTheme()
        {
            const string registryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            const string registryValueName = "AppsUseLightTheme";
            // 这里也可能是LocalMachine(HKEY_LOCAL_MACHINE)
            // see "https://www.addictivetips.com/windows-tips/how-to-enable-the-dark-theme-in-windows-10/"
            var registryValueObject = Registry.CurrentUser.OpenSubKey(registryKeyPath)?.GetValue(registryValueName);
            if (registryValueObject is null) return false;
            return (int)registryValueObject <= 0;
        }
    }
}