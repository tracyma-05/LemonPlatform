using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using LemonPlatform.BusyIndicator.Command;
using LemonPlatform.BusyIndicator.Indicator;

namespace LemonPlatform.BusyIndicator.BusyMask
{
    [TemplateVisualState(Name = VisualStates.StateHidden, GroupName = VisualStates.GroupVisibility)]
    [TemplateVisualState(Name = VisualStates.StateVisible, GroupName = VisualStates.GroupVisibility)]
    public class BusyMask : ContentControl
    {
        [Category(nameof(BusyIndicator))]
        [Description("Gets or sets whether the indicator is busy.")]
        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(nameof(IsBusy),
                typeof(bool),
                typeof(BusyMask),
                new PropertyMetadata(false, OnIsBusyChanged));

        [Description("Gets or sets whether the indicator is busy by default on startup.")]
        private bool _IsBusyATStartup;
        public bool IsBusyAtStartup
        {
            get => _IsBusyATStartup;
            set => _IsBusyATStartup = value;
        }

        [Category(nameof(BusyIndicator))]
        [Description("Gets or sets indicator content such as waiting message.")]
        public string BusyContent
        {
            get => (string)GetValue(BusyContentProperty);
            set => SetValue(BusyContentProperty, value);
        }

        public static readonly DependencyProperty BusyContentProperty =
            DependencyProperty.Register(nameof(BusyContent),
                typeof(string),
                typeof(BusyMask),
                new PropertyMetadata("Please wait..."));

        [Category(nameof(BusyIndicator))]
        [Description("Gets or sets indicator content margin.")]
        public Thickness BusyContentMargin
        {
            get => (Thickness)GetValue(BusyContentMarginProperty);
            set => SetValue(BusyContentMarginProperty, value);
        }

        public static readonly DependencyProperty BusyContentMarginProperty =
            DependencyProperty.Register(nameof(BusyContentMargin),
                typeof(Thickness),
                typeof(BusyMask),
                new PropertyMetadata(new Thickness(10)));

        [Category(nameof(BusyIndicator))]
        [Description("Gets or sets the indicator type.")]
        public IndicatorType IndicatorType
        {
            get => (IndicatorType)GetValue(IndicatorTypeProperty);
            set => SetValue(IndicatorTypeProperty, value);
        }

        public static readonly DependencyProperty IndicatorTypeProperty =
            DependencyProperty.Register(nameof(IndicatorType),
                typeof(IndicatorType),
                typeof(BusyMask),
                new PropertyMetadata(IndicatorType.Twist));

        [Category(nameof(BusyIndicator))]
        [Description("Gets or sets the control which gets focused after the wait is over.")]
        public Control FocusAfterBusy
        {
            get => (Control)GetValue(FocusAfterBusyProperty);
            set => SetValue(FocusAfterBusyProperty, value);
        }

        public static readonly DependencyProperty FocusAfterBusyProperty =
            DependencyProperty.Register(nameof(FocusAfterBusy),
                typeof(Control),
                typeof(BusyMask),
                new PropertyMetadata(null));

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var elapsedTime = DateTime.Now - _startTime;
            Time = elapsedTime.ToString(@"hh\:mm\:ss");
        }

        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(string), typeof(BusyMask), new PropertyMetadata(string.Empty));

        private DispatcherTimer _dispatcherTimer;
        private DateTime _startTime;


        static BusyMask()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyMask),
                new FrameworkPropertyMetadata(typeof(BusyMask)));
        }

        public BusyMask()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;

            CancelPropertyCommand = new BusyRelayCommand(OnCancelPropertyCommandExecuted);
        }

        private void OnCancelPropertyCommandExecuted(object parameter)
        {
            IsBusy = false;
        }

        public ICommand CancelPropertyCommand { get; }

        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyMask)d).OnIsBusyChanged(e);
        }

        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            Time = "00:00:00";
            _startTime = DateTime.Now;
            if (!(bool)e.NewValue)
            {
                _dispatcherTimer.Stop();
                if (FocusAfterBusy != null)
                {
                    FocusAfterBusy.Dispatcher.Delay(100, (_) =>
                    {
                        FocusAfterBusy.Focus();
                    });
                }
            }
            else
            {
                _dispatcherTimer.Start();
            }

            ChangeVisualState((bool)e.NewValue);
        }

        public override void OnApplyTemplate()
        {
            IsBusy = IsBusyAtStartup;
            ChangeVisualState(IsBusyAtStartup);
        }

        protected virtual void ChangeVisualState(bool isBusyContentVisible = false)
        {
            VisualStateManager.GoToState(this, isBusyContentVisible ? "Visible" : "Hidden", true);
        }
    }
}