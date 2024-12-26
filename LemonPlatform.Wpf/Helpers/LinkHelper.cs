using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LemonPlatform.Wpf.Helpers
{
    public static class LinkHelper
    {
        public static void OpenInBrowser(string? url)
        {
            if (url is not null && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }
    }
}