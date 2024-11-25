using LemonPlatform.Wpf.Commons;
using System.Runtime.InteropServices;

namespace LemonPlatform.Wpf.Helpers
{
    internal class ApplicationHelper
    {
        private static Mutex _mutex;

        public static void CheckApplicationMutex()
        {
            bool mutexResult;
            _mutex = new Mutex(true, LemonConstants.ApplicationName, out mutexResult);
            if (!mutexResult)
            {
                // 找到已经在运行的实例句柄(给出你的窗体标题名 “Deamon Club”)
                IntPtr hWndPtr = FindWindow(null, "NextPlatform");

                // 还原窗口
                ShowWindow(hWndPtr, SW_RESTORE);

                // 激活窗口
                SetForegroundWindow(hWndPtr);

                // 退出当前实例程序
                Environment.Exit(0);
            }
        }

        #region Windows API

        public const int SW_RESTORE = 9;

        [DllImport("USER32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("USER32.DLL")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion
    }
}