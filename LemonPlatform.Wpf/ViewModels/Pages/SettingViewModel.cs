using CommunityToolkit.Mvvm.ComponentModel;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.Helpers;
using MaterialDesignThemes.Wpf;

namespace LemonPlatform.Wpf.ViewModels.Pages
{
    public partial class SettingViewModel : ObservableObject, ISingletonDependency
    {
        public SettingViewModel()
        {
            InitData();
            SetTheme();
        }

        [ObservableProperty]
        private bool _isDarkTheme;
        partial void OnIsDarkThemeChanged(bool oldValue, bool newValue)
        {
            ThemeHelper.ModifyTheme(theme => theme.SetBaseTheme(IsDarkTheme ? BaseTheme.Dark : BaseTheme.Light));
        }

        [ObservableProperty]
        private bool _isColorAdjusted;
        partial void OnIsColorAdjustedChanged(bool oldValue, bool newValue)
        {
            ThemeHelper.ModifyTheme(theme =>
            {
                if (theme is Theme internalTheme)
                {
                    internalTheme.ColorAdjustment = newValue
                        ? new ColorAdjustment
                        {
                            DesiredContrastRatio = DesiredContrastRatio,
                            Contrast = ContrastValue,
                            Colors = ColorSelectionValue
                        }
                        : null;
                }
            });
        }

        [ObservableProperty]
        private float _desiredContrastRatio;
        partial void OnDesiredContrastRatioChanged(float oldValue, float newValue)
        {
            ThemeHelper.ModifyTheme(theme =>
            {
                if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                    internalTheme.ColorAdjustment.DesiredContrastRatio = newValue;
            });
        }

        [ObservableProperty]
        private Contrast _contrastValue;
        partial void OnContrastValueChanged(Contrast oldValue, Contrast newValue)
        {
            ThemeHelper.ModifyTheme(theme =>
            {
                if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                    internalTheme.ColorAdjustment.Contrast = newValue;
            });
        }

        [ObservableProperty]
        private ColorSelection _colorSelectionValue;
        partial void OnColorSelectionValueChanged(ColorSelection oldValue, ColorSelection newValue)
        {
            ThemeHelper.ModifyTheme(theme =>
            {
                if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                    internalTheme.ColorAdjustment.Colors = newValue;
            });
        }

        [ObservableProperty]
        private IEnumerable<ColorSelection> _colorSelectionValues;

        [ObservableProperty]
        public IEnumerable<Contrast> _contrastValues;

        private void InitData()
        {
            ColorSelectionValues = Enum.GetValues(typeof(ColorSelection)).Cast<ColorSelection>(); ;
            ContrastValues = Enum.GetValues(typeof(Contrast)).Cast<Contrast>();
            DesiredContrastRatio = 4.5f;
        }

        private void SetTheme()
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark;

            if (theme is Theme internalTheme)
            {
                IsColorAdjusted = internalTheme.ColorAdjustment is not null;

                var colorAdjustment = internalTheme.ColorAdjustment ?? new ColorAdjustment();
                DesiredContrastRatio = colorAdjustment.DesiredContrastRatio;
                ContrastValue = colorAdjustment.Contrast;
                ColorSelectionValue = colorAdjustment.Colors;
            }

            if (paletteHelper.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += (_, e) =>
                {
                    IsDarkTheme = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
                };
            }
        }
    }
}