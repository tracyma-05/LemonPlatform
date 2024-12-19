using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Button = System.Windows.Controls.Button;
using Control = System.Windows.Controls.Control;
using Cursors = System.Windows.Input.Cursors;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;
using RadioButton = System.Windows.Controls.RadioButton;
using Rectangle = System.Windows.Shapes.Rectangle;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TextBox = System.Windows.Controls.TextBox;

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



        private FrameworkElement frameworkElement;
        private bool isMouseUp;

        private Rect rect;

        /// <summary>
        ///     当前画笔
        /// </summary>
        private Polyline? polyLine;

        /// <summary>
        ///     当前文本
        /// </summary>
        private Border? textBorder;

        /// <summary>
        ///     当前绘制矩形
        /// </summary>
        private Border? borderRectangle;

        /// <summary>
        ///     当前箭头
        /// </summary>
        private Control? controlArrow;

        /// <summary>
        ///     绘制当前椭圆
        /// </summary>
        private Ellipse? drawEllipse;

        /// <summary>
        /// 当前的直线起点和终点
        /// </summary>
        private Point? pointStart, pointEnd;

        private double y1;

        private ControlTemplate _controlTemplate;

        private int _screenIndex;
        private readonly ScreenDPI _screenDPI;
        private ScreenCutAdorner _screenCutAdorner;

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
            if (_buttonCancel != null) _buttonCancel.Click += ButtonCancel_Click;
            if (_buttonComplete != null) _buttonComplete.Click += ButtonComplete_Click;
            if (_radioButtonRectangle != null) _radioButtonRectangle.Click += RadioButtonRectangle_Click;
            if (_radioButtonEllipse != null) _radioButtonEllipse.Click += RadioButtonEllipse_Click;
            if (_radioButtonArrow != null) _radioButtonArrow.Click += RadioButtonArrow_Click;
            if (_radioButtonInk != null) _radioButtonInk.Click += RadioButtonInk_Click;
            if (_radioButtonText != null) _radioButtonText.Click += RadioButtonText_Click;
            if (_popupBorder != null) _popupBorder.Loaded += PopupBorder_Loaded;
            if (_wrapPanel != null) _wrapPanel.PreviewMouseDown += WrapPanel_PreviewMouseDown;

            var bounds = Screen.AllScreens[_screenIndex].Bounds;
            _canvas.Width = bounds.Width;
            _canvas.Height = bounds.Height;
            _canvas.Background = new ImageBrush(ConvertBitmap(CopyScreen()));
            _border.Opacity = 0;

            Loaded += ScreenCut_Loaded;
            _controlTemplate = (ControlTemplate)FindResource("SC.PART_DrawArrow");
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
                var pngEncoder = new PngBitmapEncoder();
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

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) => OnCanceled();

        private void ButtonComplete_Click(object sender, RoutedEventArgs e)
        {
            var bitmap = CutBitmap();
            CutCompleted?.Invoke(bitmap);
            Close();
        }

        private void RadioButtonRectangle_Click(object sender, RoutedEventArgs e) => RadioButtonChecked(_radioButtonRectangle, ScreenCutMouseType.DrawRectangle);

        private void RadioButtonText_Click(object sender, RoutedEventArgs e) => RadioButtonChecked(_radioButtonText, ScreenCutMouseType.DrawText);

        private void RadioButtonInk_Click(object sender, RoutedEventArgs e) => RadioButtonChecked(_radioButtonInk, ScreenCutMouseType.DrawInk);

        private void RadioButtonArrow_Click(object sender, RoutedEventArgs e) => RadioButtonChecked(_radioButtonArrow, ScreenCutMouseType.DrawArrow);

        private void RadioButtonEllipse_Click(object sender, RoutedEventArgs e) => RadioButtonChecked(_radioButtonEllipse, ScreenCutMouseType.DrawEllipse);

        private void PopupBorder_Loaded(object sender, RoutedEventArgs e) => _popup.HorizontalOffset = -_popupBorder.ActualWidth / 3;

        private void WrapPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is RadioButton radioButton)
            {
                _currentBrush = radioButton.Background;
            }
        }

        private void ScreenCut_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        #endregion

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (e.Source is RadioButton) return;
            if (pointStart is null) return;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var current = e.GetPosition(_canvas);
                switch (_screenCutMouseType)
                {
                    case ScreenCutMouseType.DrawMouse:
                        MoveAllRectangle(current);
                        break;
                    case ScreenCutMouseType.MoveMouse:
                        MoveRect(current);
                        break;
                    case ScreenCutMouseType.DrawRectangle:
                    case ScreenCutMouseType.DrawEllipse:
                        DrawMultipleControl(current);
                        break;
                    case ScreenCutMouseType.DrawArrow:
                        DrawArrowControl(current);
                        break;
                    case ScreenCutMouseType.DrawInk:
                        DrawInkControl(current);
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.Source is RadioButton) return;
            if (CaptureScreenId == -1) CaptureScreenId = _screenIndex;
            if (CaptureScreenId != -1 && CaptureScreenId != _screenIndex)
            {
                e.Handled = true;
                return;
            }

            var point = e.GetPosition(_canvas);
            if (!isMouseUp)
            {
                pointStart = point;
                _screenCutMouseType = ScreenCutMouseType.DrawMouse;
                _editBar.Visibility = Visibility.Hidden;
                pointEnd = pointStart;
                rect = new Rect(pointStart.Value, pointEnd.Value);
            }
            else
            {
                if (point.X < rect.Left || point.X > rect.Right) return;
                if (point.Y < rect.Top || point.Y > rect.Bottom) return;
                pointStart = point;
                if (textBorder != null) Focus();
                switch (_screenCutMouseType)
                {
                    case ScreenCutMouseType.DrawText:
                        y1 = point.Y;
                        DrawText();

                        break;

                    default:
                        Focus();
                        break;
                }
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape) OnCanceled();
            else if (e.Key == Key.Delete)
            {
                if (_canvas.Children.Count > 0) _canvas.Children.Remove(frameworkElement);
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.Z) && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (_canvas.Children.Count > 0) _canvas.Children.Remove(_canvas.Children[_canvas.Children.Count - 1]);
            }
        }

        private void DrawInkControl(Point current)
        {
            CheckPoint(current);
            if (current.X >= rect.Left &&
                current.X <= rect.Right &&
                current.Y >= rect.Top &&
                current.Y <= rect.Bottom)
            {
                if (polyLine == null)
                {
                    polyLine = new Polyline();
                    polyLine.Stroke = _currentBrush == null ? Brushes.Red : _currentBrush;
                    polyLine.Cursor = Cursors.Hand;
                    polyLine.StrokeThickness = 3;
                    polyLine.StrokeLineJoin = PenLineJoin.Round;
                    polyLine.StrokeStartLineCap = PenLineCap.Round;
                    polyLine.StrokeEndLineCap = PenLineCap.Round;
                    polyLine.MouseLeftButtonDown += (s, e) =>
                    {
                        _radioButtonInk.IsChecked = true;
                        RadioButtonInk_Click(null, null);
                        SelectElement();
                        frameworkElement = s as Polyline;
                        frameworkElement.Opacity = 0.7;
                    };
                    _canvas.Children.Add(polyLine);
                }

                polyLine.Points.Add(current);
            }
        }

        private void DrawArrowControl(Point current)
        {
            CheckPoint(current);
            if (_screenCutMouseType != ScreenCutMouseType.DrawArrow)
                return;

            if (pointStart is null)
                return;

            var vPoint = pointStart.Value;

            var drawArrow = new Rect(vPoint, current);
            if (controlArrow == null)
            {
                controlArrow = new Control();
                controlArrow.Background = _currentBrush == null ? Brushes.Red : _currentBrush;
                controlArrow.Template = _controlTemplate;
                controlArrow.Cursor = Cursors.Hand;
                controlArrow.Tag = _tag;
                controlArrow.MouseLeftButtonDown += (s, e) =>
                {
                    _radioButtonArrow.IsChecked = true;
                    RadioButtonArrow_Click(null, null);
                    SelectElement();
                    frameworkElement = s as Control;
                    frameworkElement.Opacity = .7;
                };
                _canvas.Children.Add(controlArrow);
                Canvas.SetLeft(controlArrow, drawArrow.Left);
                Canvas.SetTop(controlArrow, drawArrow.Top - 7.5);
            }

            var rotate = new RotateTransform();
            var renderOrigin = new Point(0, .5);
            controlArrow.RenderTransformOrigin = renderOrigin;
            controlArrow.RenderTransform = rotate;
            rotate.Angle = ControlsHelper.CalculateAngle(vPoint, current);
            if (current.X < rect.Left
                || current.X > rect.Right 
                || current.Y < rect.Top 
                || current.Y > rect.Bottom)
            {
                if (current.X >= vPoint.X && current.Y < vPoint.Y)
                {
                    var a1 = (current.Y - vPoint.Y) / (current.X - vPoint.X);
                    var b1 = vPoint.Y - a1 * vPoint.X;
                    var xTop = (rect.Top - b1) / a1;
                    var yRight = a1 * rect.Right + b1;

                    if (xTop <= rect.Right)
                    {
                        current.X = xTop;
                        current.Y = rect.Top;
                    }
                    else
                    {
                        current.X = rect.Right;
                        current.Y = yRight;
                    }
                }
                else if (current.X > vPoint.X && current.Y > vPoint.Y)
                {
                    var a1 = (current.Y - vPoint.Y) / (current.X - vPoint.X);
                    var b1 = vPoint.Y - a1 * vPoint.X;
                    var xBottom = (rect.Bottom - b1) / a1;
                    var yRight = a1 * rect.Right + b1;

                    if (xBottom <= rect.Right)
                    {
                        current.X = xBottom;
                        current.Y = rect.Bottom;
                    }
                    else
                    {
                        current.X = rect.Right;
                        current.Y = yRight;
                    }
                }
                else if (current.X < vPoint.X && current.Y < vPoint.Y)
                {
                    var a1 = (current.Y - vPoint.Y) / (current.X - vPoint.X);
                    var b1 = vPoint.Y - a1 * vPoint.X;
                    var xTop = (rect.Top - b1) / a1;
                    var yLeft = a1 * rect.Left + b1;
                    if (xTop >= rect.Left)
                    {
                        current.X = xTop;
                        current.Y = rect.Top;
                    }
                    else
                    {
                        current.X = rect.Left;
                        current.Y = yLeft;
                    }
                }
                else if (current.X < vPoint.X && current.Y > vPoint.Y)
                {
                    var a1 = (current.Y - vPoint.Y) / (current.X - vPoint.X);
                    var b1 = vPoint.Y - a1 * vPoint.X;
                    var xBottom = (rect.Bottom - b1) / a1;
                    var yLeft = a1 * rect.Left + b1;

                    if (xBottom <= rect.Left)
                    {
                        current.X = rect.Left;
                        current.Y = yLeft;
                    }
                    else
                    {
                        current.X = xBottom;
                        current.Y = rect.Bottom;
                    }
                }
            }

            var x = current.X - vPoint.X;
            var y = current.Y - vPoint.Y;
            var width = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            width = width < 15 ? 15 : width;
            controlArrow.Width = width;
        }

        private void DrawMultipleControl(Point current)
        {
            CheckPoint(current);
            if (pointStart is null) return;

            var vPoint = pointStart.Value;
            var drawRect = new Rect(vPoint, current);
            switch (_screenCutMouseType)
            {
                case ScreenCutMouseType.DrawRectangle:
                    if (borderRectangle == null)
                    {
                        borderRectangle = new Border
                        {
                            BorderBrush = _currentBrush == null ? Brushes.Red : _currentBrush,
                            BorderThickness = new Thickness(3),
                            CornerRadius = new CornerRadius(3),
                            Tag = _tag,
                            Cursor = Cursors.Hand
                        };

                        borderRectangle.MouseLeftButtonDown += (s, e) =>
                        {
                            _radioButtonRectangle.IsChecked = true;
                            RadioButtonRectangle_Click(null, null);
                            SelectElement();
                            frameworkElement = s as Border;
                            frameworkElement.Opacity = 0.7;
                        };

                        _canvas.Children.Add(borderRectangle);
                    }

                    break;
                case ScreenCutMouseType.DrawEllipse:
                    if (drawEllipse == null)
                    {
                        drawEllipse = new Ellipse
                        {
                            Stroke = _currentBrush == null ? Brushes.Red : _currentBrush,
                            StrokeThickness = 3,
                            Tag = _tag,
                            Cursor = Cursors.Hand
                        };

                        drawEllipse.MouseLeftButtonDown += (s, e) =>
                        {
                            _radioButtonEllipse.IsChecked = true;
                            RadioButtonEllipse_Click(null, null);
                            SelectElement();
                            frameworkElement = s as Ellipse;
                            frameworkElement.Opacity = .7;
                        };

                        _canvas.Children.Add(drawEllipse);
                    }

                    break;
            }

            var _borderLeft = drawRect.Left - Canvas.GetLeft(_border);
            if (_borderLeft < 0) _borderLeft = Math.Abs(_borderLeft);
            if (drawRect.Width + _borderLeft < _border.ActualWidth)
            {
                var wLeft = Canvas.GetLeft(_border) + _border.ActualWidth;
                var left = drawRect.Left < Canvas.GetLeft(_border) ? Canvas.GetLeft(_border) : drawRect.Left > wLeft ? wLeft : drawRect.Left;
                if (borderRectangle != null)
                {
                    borderRectangle.Width = drawRect.Width;
                    Canvas.SetLeft(borderRectangle, left);
                }

                if (drawEllipse != null)
                {
                    drawEllipse.Width = drawRect.Width;
                    Canvas.SetLeft(drawEllipse, left);
                }
            }

            var _borderTop = drawRect.Top - Canvas.GetTop(_border);
            if (_borderTop < 0) _borderTop = Math.Abs(_borderTop);
            if (drawRect.Height + _borderTop < _border.ActualHeight)
            {
                var hTop = Canvas.GetTop(_border) + _border.Height;
                var top = drawRect.Top < Canvas.GetTop(_border) ? Canvas.GetTop(_border) : drawRect.Top > hTop ? hTop : drawRect.Top;
                if (borderRectangle != null)
                {
                    borderRectangle.Height = drawRect.Height;
                    Canvas.SetTop(borderRectangle, top);
                }

                if (drawEllipse != null)
                {
                    drawEllipse.Height = drawRect.Height;
                    Canvas.SetTop(drawEllipse, top);
                }
            }
        }

        private void MoveRect(Point current)
        {
            if (pointStart is null) return;
            var vPoint = pointStart.Value;
            if (current != vPoint)
            {
                var vector = Point.Subtract(current, vPoint);
                var left = Canvas.GetLeft(_border) + vector.X;
                var top = Canvas.GetTop(_border) + vector.Y;
                if (left <= 0) left = 0;
                if (top <= 0) top = 0;
                if (left + _border.Width >= _canvas.ActualWidth) left = _canvas.ActualWidth - _border.ActualWidth;
                if (top + _border.Height >= _canvas.ActualHeight) top = _canvas.ActualHeight - _border.ActualHeight;
                pointStart = current;

                Canvas.SetLeft(_border, left);
                Canvas.SetTop(_border, top);
                rect = new Rect(new Point(left, top), new Point(left + _border.Width, top + _border.Height));
                _rectangleLeft.Height = _canvas.ActualHeight;
                _rectangleLeft.Width = left <= 0 ? 0 : left >= _canvas.ActualWidth ? _canvas.ActualWidth : left;

                Canvas.SetLeft(_rectangleTop, _rectangleLeft.Width);
                _rectangleTop.Height = top <= 0 ? 0 : top >= _canvas.ActualHeight ? _canvas.ActualHeight : top;

                Canvas.SetLeft(_rectangleRight, left + _border.Width);
                var wRight = _canvas.ActualWidth - (_border.Width + _rectangleLeft.Width);
                _rectangleRight.Width = wRight <= 0 ? 0 : wRight;
                _rectangleRight.Height = _canvas.ActualHeight;

                Canvas.SetLeft(_rectangleBottom, _rectangleLeft.Width);
                Canvas.SetTop(_rectangleBottom, top + _border.Height);
                _rectangleBottom.Width = _border.Width;
                var hBottom = _canvas.ActualHeight - (top + _border.Height);
                _rectangleBottom.Height = hBottom <= 0 ? 0 : hBottom;
            }
        }

        private void MoveAllRectangle(Point current)
        {
            if (pointStart is null) return;
            var vPoint = pointStart.Value;

            pointEnd = current;
            var vEndPoint = current;

            rect = new Rect(vPoint, vEndPoint);
            _rectangleLeft.Width = rect.X < 0 ? 0 : rect.X > _canvas.ActualWidth ? _canvas.ActualWidth : rect.X;
            _rectangleLeft.Height = _canvas.Height;

            Canvas.SetLeft(_rectangleTop, _rectangleLeft.Width);
            _rectangleTop.Width = rect.Width;
            var h = 0.0;
            if (current.Y < vPoint.Y)
                h = current.Y;
            else
                h = current.Y - rect.Height;

            _rectangleTop.Height = h < 0 ? 0 : h > _canvas.ActualHeight ? _canvas.ActualHeight : h;

            Canvas.SetLeft(_rectangleRight, _rectangleLeft.Width + rect.Width);
            var rWidth = _canvas.Width - (rect.Width + _rectangleLeft.Width);
            _rectangleRight.Width = rWidth < 0 ? 0 : rWidth > _canvas.ActualWidth ? _canvas.ActualWidth : rWidth;

            _rectangleRight.Height = _canvas.Height;

            Canvas.SetLeft(_rectangleBottom, _rectangleLeft.Width);
            Canvas.SetTop(_rectangleBottom, rect.Height + _rectangleTop.Height);
            _rectangleBottom.Width = rect.Width;
            var rBottomHeight = _canvas.Height - (rect.Height + _rectangleTop.Height);
            _rectangleBottom.Height = rBottomHeight < 0 ? 0 : rBottomHeight;

            _border.Height = rect.Height;
            _border.Width = rect.Width;
            Canvas.SetLeft(_border, rect.X);
            Canvas.SetTop(_border, rect.Y);

            if (adornerLayer != null) return;
            _border.Opacity = 1;
            adornerLayer = AdornerLayer.GetAdornerLayer(_border);
            _screenCutAdorner = new ScreenCutAdorner(_border);
            _screenCutAdorner.PreviewMouseDown += (s, e) =>
            {
                Restore();
                RetoreRadioButton();
            };
            adornerLayer.Add(_screenCutAdorner);
            _border.SizeChanged += Border_SizeChanged;
        }

        private void CheckPoint(Point current)
        {
            if (current == pointStart) return;
            if (current.X > rect.BottomRight.X || current.Y > rect.BottomRight.Y) return;
        }

        private void RetoreRadioButton()
        {
            _radioButtonRectangle.IsChecked = false;
            _radioButtonEllipse.IsChecked = false;
        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (isMouseUp)
            {
                var left = Canvas.GetLeft(_border);
                var top = Canvas.GetTop(_border);
                var beignPoint = new Point(left, top);
                var endPoint = new Point(left + _border.ActualWidth, top + _border.ActualHeight);
                rect = new Rect(beignPoint, endPoint);
                pointStart = beignPoint;
                MoveAllRectangle(endPoint);
            }

            EditBarPosition();
        }

        private void EditBarPosition()
        {
            _editBar.Visibility = Visibility.Visible;
            Canvas.SetLeft(_editBar, rect.X + rect.Width - _editBar.ActualWidth);
            var y = Canvas.GetTop(_border) + _border.ActualHeight + _editBar.ActualHeight + _popupBorder.ActualHeight + 24;
            if (y > _canvas.ActualHeight && Canvas.GetTop(_border) > _editBar.ActualHeight) y = Canvas.GetTop(_border) - _editBar.ActualHeight - 8;
            else if (y > _canvas.ActualHeight && Canvas.GetTop(_border) < _editBar.ActualHeight) y = _border.ActualHeight - _editBar.ActualHeight - 8;
            else y = Canvas.GetTop(_border) + _border.ActualHeight + 8;

            Canvas.SetTop(_editBar, y);
            if (_popup != null && _popup.IsOpen)
            {
                _popup.IsOpen = false;
                _popup.IsOpen = true;
            }
        }

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

        private void OnCanceled()
        {
            CutCanceled?.Invoke();
            Close();
        }

        private void RadioButtonChecked(RadioButton radioButton, ScreenCutMouseType screenCutMouseType)
        {
            if (radioButton.IsChecked is true)
            {
                _screenCutMouseType = screenCutMouseType;
                _border.Cursor = Cursors.Arrow;
                if (_popup.PlacementTarget != null && _popup.IsOpen) _popup.IsOpen = false;
                _popup.PlacementTarget = radioButton;
                _popup.IsOpen = true;
                DisposeControl();
            }
            else
            {
                if (_screenCutMouseType == screenCutMouseType) Restore();
            }
        }

        private void DisposeControl()
        {
            polyLine = null;
            textBorder = null;
            borderRectangle = null;
            drawEllipse = null;
            controlArrow = null;
            pointStart = null;
            pointEnd = null;
        }

        private void Restore()
        {
            _border.Cursor = Cursors.SizeAll;
            if (_screenCutMouseType == ScreenCutMouseType.Default) return;
            _screenCutMouseType = ScreenCutMouseType.Default;
            if (_popup.PlacementTarget != null && _popup.IsOpen) _popup.IsOpen = false;
        }

        private BitmapSource ConvertBitmap(Bitmap bitmap)
        {
            var hBitmap = bitmap.GetHbitmap();
            var image = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            return image;
        }

        private void DrawText()
        {
            if (pointStart.Value.X < rect.Right && pointStart.Value.X > rect.Left && pointStart.Value.Y > rect.Top && pointStart.Value.Y < rect.Bottom)
            {
                var currentWAndX = pointStart.Value.X + 40;
                if (textBorder == null)
                {
                    textBorder = new Border
                    {
                        BorderBrush = _currentBrush == null ? Brushes.Red : _currentBrush,
                        BorderThickness = new Thickness(1),
                        Tag = _tag
                    };

                    textBorder.SizeChanged += TextBorder_SizeChanged;
                    textBorder.PreviewMouseLeftButtonDown += TextBorder_PreviewMouseLeftButtonDown;

                    var textBox = new TextBox
                    {
                        Style = null,
                        Background = null,
                        BorderThickness = new Thickness(0),
                        Foreground = textBorder.BorderBrush,
                        FontFamily = DrawingContextHelper.FontFamily,
                        FontSize = 16,
                        TextWrapping = TextWrapping.Wrap,
                        FontWeight = FontWeights.Normal,
                        MinWidth = _width,
                        MaxWidth = rect.Right - pointStart.Value.X,
                        MaxHeight = rect.Height - 4,
                        Cursor = Cursors.Hand,
                        Padding = new Thickness(4)
                    };

                    textBox.LostKeyboardFocus += TextBox_LostKeyboardFocus;

                    textBorder.Child = textBox;
                    _canvas.Children.Add(textBorder);
                    textBox.Focus();

                    var x = pointStart.Value.X;
                    if (currentWAndX > rect.Right) x = x - (currentWAndX - rect.Right);
                    Canvas.SetLeft(textBorder, x - 2);
                    Canvas.SetTop(textBorder, pointStart.Value.Y - 2);
                }
            }
        }

        private void TextBorder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _radioButtonText.IsChecked = true;
            RadioButtonText_Click(null, null);
            SelectElement();

            var border = sender as Border;
            frameworkElement = border;
            frameworkElement.Opacity = 0.7;
            border.BorderThickness = new Thickness(1);
        }

        private void TextBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var border = sender as Border;
            var y = y1;
            if (y + border.ActualHeight > rect.Bottom)
            {
                var v = Math.Abs(rect.Bottom - (y + border.ActualHeight));
                var y1 = y - v;
                Canvas.SetTop(border, y1 + 2);
            }
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var parent = VisualTreeHelper.GetParent(textBox);
            if (parent != null && parent is Border border)
            {
                border.BorderThickness = new Thickness(0);
                if (string.IsNullOrEmpty(textBox.Text)) _canvas.Children.Remove(border);
            }
        }

        private void SelectElement()
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(_canvas); i++)
            {
                var child = VisualTreeHelper.GetChild(_canvas, i);
                if (child is FrameworkElement frameworkElement && frameworkElement.Tag != null)
                {
                    if (frameworkElement.Tag.ToString() == _tag) frameworkElement.Opacity = 1;
                }
            }
        }

        #endregion
    }
}