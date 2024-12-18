using System.Runtime.InteropServices;

namespace LemonPlatform.CustomControls.Controls.ScreenCuts
{
    public static class ScreenExtension
    {
        [DllImport(WIN32.User32)]
        private static extern IntPtr MonitorFromPoint([In] Point pt, [In] uint dwFlags);

        [DllImport(WIN32.Shcore)]
        private static extern IntPtr GetDpiForMonitor([In] IntPtr hMonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

        public static void GetDPI(this Screen screen, DpiType dpiType, out uint dpiX, out uint dpiY)
        {
            var x = screen.Bounds.Left + 1;
            var y = screen.Bounds.Top + 1;
            var pnt = new Point(x, y);

            var mon = MonitorFromPoint(pnt, 2);
            GetDpiForMonitor(mon, dpiType, out dpiX, out dpiY);
        }
    }
}