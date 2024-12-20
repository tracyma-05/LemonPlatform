using System.Windows.Media.Imaging;
using Clipboard = System.Windows.Clipboard;

namespace LemonPlatform.CustomControls.Controls.ScreenCuts
{
    public class ScreenCapture
    {
        private readonly bool _copyToClipboard;
        private readonly List<ScreenCut> _screenCuts = new List<ScreenCut>();
        private bool isCapturing;

        private static ScreenCapture _instance;
        private static readonly object _lock = new object();

        #region delegate & event

        public delegate void ScreenShotDone(CroppedBitmap bitmap);
        public delegate void ScreenShotCanceled();

        public event ScreenShotDone CaptureCompleted;
        public event ScreenShotCanceled CaptureCanceled;

        #endregion

        public static ScreenCapture GetInstance(bool copyToClipboard = true)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ScreenCapture(copyToClipboard);
                    }
                }
            }

            return _instance;
        }

        private ScreenCapture(bool copyToClipboard = true)
        {
            _copyToClipboard = copyToClipboard;
            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                var screenCut = CaptureScreen(i);
                _screenCuts.Add(screenCut);
            }
        }

        public void Capture()
        {
            if (isCapturing) return;
            if (!_screenCuts.Any())
            {
                for (int i = 0; i < Screen.AllScreens.Length; i++)
                {
                    var screenCut = CaptureScreen(i);
                    _screenCuts.Add(screenCut);
                }
            }

            foreach (var screen in _screenCuts)
            {
                screen.Show();
                screen.Activate();
            }

            isCapturing = true;
        }

        private ScreenCut CaptureScreen(int index)
        {
            var screenCut = new ScreenCut(index);
            screenCut.CutCompleted += ScreenCutCutCompleted;
            screenCut.CutCanceled += ScreenCutCutCanceled;
            screenCut.Closed += ScreenCutClosed;

            return screenCut;
        }

        private void ScreenCutClosed(object? sender, EventArgs e)
        {
            var current = sender as ScreenCut;
            if (current != null && _screenCuts.Contains(current)) _screenCuts.Remove(current);

            CloseScreenCuts();
            ScreenCut.ClearCaptureScreenId();
            isCapturing = false;
        }

        private void ScreenCutCutCanceled()
        {
            CaptureCanceled?.Invoke();
            isCapturing = false;
        }

        private void ScreenCutCutCompleted(CroppedBitmap bitmap)
        {
            CaptureCompleted?.Invoke(bitmap);
            if (_copyToClipboard) Clipboard.SetImage(bitmap);
            isCapturing = false;
        }

        private void CloseScreenCuts()
        {
            if (!_screenCuts.Any()) return;
            while (_screenCuts.Any())
            {
                _screenCuts[0].Close();
            }

            _screenCuts.Clear();
            isCapturing = false;
        }
    }
}