using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RadioButton = System.Windows.Controls.RadioButton;
using Button = System.Windows.Controls.Button;
using Cursors = System.Windows.Input.Cursors;
using Control = System.Windows.Controls.Control;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TextBox = System.Windows.Controls.TextBox;
using Point = System.Windows.Point;
using Brush = System.Windows.Media.Brush;
using Rectangle = System.Windows.Shapes.Rectangle;
using Brushes = System.Windows.Media.Brushes;
using System.Windows.Input;
using System.IO;
using System.Windows.Media;

namespace LemonPlatform.CustomControls.Controls.ScreenCuts
{
    [TemplatePart(Name = CanvasTemplateName, Type = typeof(Canvas))]
    [TemplatePart(Name = RectangleLeftTemplateName, Type = typeof(Rectangle))]
    [TemplatePart(Name = RectangleTopTemplateName, Type = typeof(Rectangle))]
    [TemplatePart(Name = RectangleRightTemplateName, Type = typeof(Rectangle))]
    [TemplatePart(Name = RectangleBottomTemplateName, Type = typeof(Rectangle))]
    [TemplatePart(Name = BorderTemplateName, Type = typeof(Border))]
    [TemplatePart(Name = EditBarTemplateName, Type = typeof(Border))]
    [TemplatePart(Name = ButtonSaveTemplateName, Type = typeof(Button))]
    [TemplatePart(Name = ButtonCancelTemplateName, Type = typeof(Button))]
    [TemplatePart(Name = ButtonCompleteTemplateName, Type = typeof(Button))]
    [TemplatePart(Name = RadioButtonRectangleTemplateName, Type = typeof(RadioButton))]
    [TemplatePart(Name = RadioButtonEllipseTemplateName, Type = typeof(RadioButton))]
    [TemplatePart(Name = RadioButtonArrowTemplateName, Type = typeof(RadioButton))]
    [TemplatePart(Name = RadioButtonInkTemplateName, Type = typeof(RadioButton))]
    [TemplatePart(Name = RadioButtonTextTemplateName, Type = typeof(RadioButton))]
    [TemplatePart(Name = PopupTemplateName, Type = typeof(Popup))]
    [TemplatePart(Name = PopupBorderTemplateName, Type = typeof(Border))]
    [TemplatePart(Name = WrapPanelColorTemplateName, Type = typeof(WrapPanel))]
    public class ScreenCut : Window, IDisposable
    {
        private const string CanvasTemplateName = "PART_Canvas";
        private const string RectangleLeftTemplateName = "PART_RectangleLeft";
        private const string RectangleTopTemplateName = "PART_RectangleTop";
        private const string RectangleRightTemplateName = "PART_RectangleRight";
        private const string RectangleBottomTemplateName = "PART_RectangleBottom";
        private const string BorderTemplateName = "PART_Border";
        private const string EditBarTemplateName = "PART_EditBar";
        private const string ButtonSaveTemplateName = "PART_ButtonSave";
        private const string ButtonCancelTemplateName = "PART_ButtonCancel";
        private const string ButtonCompleteTemplateName = "PART_ButtonComplete";
        private const string RadioButtonRectangleTemplateName = "PART_RadioButtonRectangle";
        private const string RadioButtonEllipseTemplateName = "PART_RadioButtonEllipse";
        private const string RadioButtonArrowTemplateName = "PART_RadioButtonArrow";
        private const string RadioButtonInkTemplateName = "PART_RadioButtonInk";
        private const string RadioButtonTextTemplateName = "PART_RadioButtonText";
        private const string PopupTemplateName = "PART_Popup";
        private const string PopupBorderTemplateName = "PART_PopupBorder";
        private const string WrapPanelColorTemplateName = "PART_WrapPanelColor";

        private const string _tag = "Draw";
        private const int _width = 40;
        private Border? _border, _editBar, _popupBorder;
        private Button? _buttonSave, _buttonCancel, _buttonComplete;
        private Canvas? _canvas;

        private Brush _currentBrush;

        private Popup? _popup;

        private RadioButton? _radioButtonRectangle,
            _radioButtonEllipse,
            _radioButtonArrow,
            _radioButtonInk,
            _radioButtonText;

        private Rectangle? _rectangleLeft, _rectangleTop, _rectangleRight, _rectangleBottom;
        private WrapPanel? _wrapPanel;
        private AdornerLayer adornerLayer;

        /// <summary>
        ///     当前绘制矩形
        /// </summary>
        private Border borderRectangle;

        /// <summary>
        ///     当前箭头
        /// </summary>
        private Control controlArrow;

        private ControlTemplate controlTemplate;

        /// <summary>
        ///     绘制当前椭圆
        /// </summary>
        private Ellipse drawEllipse;

        private FrameworkElement frameworkElement;
        private bool isMouseUp;
        private Point? pointStart, pointEnd;
        private Rect rect;

        /// <summary>
        ///     当前画笔
        /// </summary>
        private Polyline polyLine;

        private int _screenIndex;
        private readonly ScreenDPI _screenDPI;
        private readonly ScreenCutAdorner _screenCutAdorner;

        #region delegate & event

        public delegate void ScreenShotDone(CroppedBitmap bitmap);
        public delegate void ScreenShotCanceled();

        public event ScreenShotDone CutCompleted;
        public event ScreenShotCanceled CutCanceled;

        #endregion

        private ScreenCutMouseType _screenCutMouseType = ScreenCutMouseType.Default;


        static ScreenCut()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScreenCut), new FrameworkPropertyMetadata(typeof(ScreenCut)));
        }

        public ScreenCut(int index)
        {
            _screenIndex = index;
            Left = Screen.AllScreens[index].WorkingArea.Left;
            ShowInTaskbar = false;
            _screenDPI = GetScreenDPI(index);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _canvas = GetTemplateChild(CanvasTemplateName) as Canvas;
            _rectangleLeft = GetTemplateChild(RectangleLeftTemplateName) as Rectangle;
            _rectangleTop = GetTemplateChild(RectangleTopTemplateName) as Rectangle;
            _rectangleRight = GetTemplateChild(RectangleRightTemplateName) as Rectangle;
            _rectangleBottom = GetTemplateChild(RectangleBottomTemplateName) as Rectangle;
            _border = GetTemplateChild(BorderTemplateName) as Border;
            _editBar = GetTemplateChild(EditBarTemplateName) as Border;
            _buttonSave = GetTemplateChild(ButtonSaveTemplateName) as Button;
            _buttonCancel = GetTemplateChild(ButtonCancelTemplateName) as Button;
            _buttonComplete = GetTemplateChild(ButtonCompleteTemplateName) as Button;
            _radioButtonRectangle = GetTemplateChild(RadioButtonRectangleTemplateName) as RadioButton;
            _radioButtonEllipse = GetTemplateChild(RadioButtonEllipseTemplateName) as RadioButton;
            _radioButtonArrow = GetTemplateChild(RadioButtonArrowTemplateName) as RadioButton;
            _radioButtonInk = GetTemplateChild(RadioButtonInkTemplateName) as RadioButton;
            _radioButtonText = GetTemplateChild(RadioButtonTextTemplateName) as RadioButton;
            _popup = GetTemplateChild(PopupTemplateName) as Popup;
            _popupBorder = GetTemplateChild(PopupBorderTemplateName) as Border;
            _wrapPanel = GetTemplateChild(WrapPanelColorTemplateName) as WrapPanel;

            if (_border != null) _border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            if (_buttonSave != null) _buttonSave.Click += ButtonSave_Click;
        }



        #region event handler

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_screenCutMouseType == ScreenCutMouseType.Default) _screenCutMouseType = ScreenCutMouseType.MoveMouse;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                FileName = $"LemonPlatform-{DateTime.Now:yyyyMMddHHmmss}.jpg",
                DefaultExt = ".jpg",
                Filter = "image file|*.jpg"
            };

            if (dlg.ShowDialog() == true)
            {
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(CutBitmap()));
                using (var fs = File.OpenWrite(dlg.FileName))
                {
                    pngEncoder.Save(fs);
                    fs.Dispose();
                    fs.Close();
                    Close();
                }
            }
        }

        #endregion


        #endregion

        #region static

        private static int CaptureScreenId = -1;

        public static void ClearCaptureScreenId() => CaptureScreenId = -1;

        #endregion

        #region private

        private ScreenDPI GetScreenDPI(int screenIndex)
        {
            var dpi = new ScreenDPI();
            Screen.AllScreens[screenIndex].GetDPI(DpiType.Effective, out dpi.DpiX, out dpi.DpiY);
            dpi.ScaleX = (dpi.DpiX / 0.96f) / 100;
            dpi.ScaleY = (dpi.DpiY / 0.96f) / 100;

            return dpi;
        }

        private Bitmap CopyScreen()
        {
            var bounds = Screen.AllScreens[_screenIndex].Bounds;

            Left = bounds.Left / _screenDPI.ScaleX;
            Top = bounds.Top / _screenDPI.ScaleY;
            Width = bounds.Width / _screenDPI.ScaleX;
            Height = bounds.Height / _screenDPI.ScaleY;

            _canvas.Width = bounds.Width / _screenDPI.ScaleX;
            _canvas.Height = bounds.Height / _screenDPI.ScaleY;
            _canvas.SetValue(LeftProperty, Left);
            _canvas.SetValue(TopProperty, Top);

            var sc = new Bitmap(bounds.Width, bounds.Height);
            using var g = Graphics.FromImage(sc);
            g.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);

            return sc;
        }

        private CroppedBitmap CutBitmap()
        {
            _border.Visibility = Visibility.Collapsed;
            _editBar.Visibility = Visibility.Collapsed;
            _rectangleLeft.Visibility = Visibility.Collapsed;
            _rectangleTop.Visibility = Visibility.Collapsed;
            _rectangleRight.Visibility = Visibility.Collapsed;
            _rectangleBottom.Visibility = Visibility.Collapsed;
            var renderTargetBitmap = new RenderTargetBitmap((int)_canvas.Width, (int)_canvas.Height, 96d, 96d, PixelFormats.Default);
            renderTargetBitmap.Render(_canvas);

            return new CroppedBitmap(renderTargetBitmap, new Int32Rect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
        }

        #endregion
    }
}