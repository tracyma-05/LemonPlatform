using CommunityToolkit.Mvvm.ComponentModel;
using LemonPlatform.Core.Commons;
using LemonPlatform.Core.Databases;
using LemonPlatform.Core.Databases.Models;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.Configs;
using LemonPlatform.Wpf.Helpers;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LemonPlatform.Wpf.ViewModels.Pages
{
    public partial class SettingViewModel : ObservableObject, ISingletonDependency
    {
        private readonly LemonDbContext _lemonDbContext;
        public SettingViewModel(LemonDbContext lemonDbContext)
        {
            _lemonDbContext = lemonDbContext;

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

            UpdateThemeConfig();
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

            UpdateThemeConfig();
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

            UpdateThemeConfig();
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

            UpdateThemeConfig();
        }

        [ObservableProperty]
        private IEnumerable<ColorSelection> _colorSelectionValues;

        [ObservableProperty]
        public IEnumerable<Contrast> _contrastValues;

        private async void InitData()
        {
            ColorSelectionValues = Enum.GetValues(typeof(ColorSelection)).Cast<ColorSelection>(); ;
            ContrastValues = Enum.GetValues(typeof(Contrast)).Cast<Contrast>();
            var dtNow = DateTime.Now;

            var themePreference = await _lemonDbContext.UserPreference.FirstOrDefaultAsync(x => x.Id == LemonConstants.ThemeConfigId);
            if (themePreference == null)
            {
                var paletteHelper = new PaletteHelper();
                var systemTheme = paletteHelper.GetTheme();
                var isDarkTheme = systemTheme.GetBaseTheme() == BaseTheme.Dark;
                var themeConfig = new ThemeConfig
                {
                    IsDarkTheme = isDarkTheme,
                    IsColorAdjusted = false,
                    DesiredContrastRatio = 4.5f,
                    ContrastValue = Contrast.None,
                    ColorSelectionValue = ColorSelection.None,
                };

                var type = GetType();
                themePreference = new UserPreference
                {
                    Id = LemonConstants.ThemeConfigId,
                    Content = JsonSerializer.Serialize(themeConfig),
                    UserId = LemonConstants.GuestUserId,
                    CreatedAt = dtNow,
                    LastModifiedAt = dtNow,
                    ModuleName = $"{type.Module.Name}-{type.Namespace}-{type.Name}",
                };

                await _lemonDbContext.AddAsync(themePreference);
                await _lemonDbContext.SaveChangesAsync();
            }

            var theme = JsonSerializer.Deserialize<ThemeConfig>(themePreference.Content);
            if (theme != null)
            {
                DesiredContrastRatio = theme.DesiredContrastRatio;
                IsDarkTheme = theme.IsDarkTheme;
                IsColorAdjusted = theme.IsColorAdjusted;
                ContrastValue = theme.ContrastValue;
                ColorSelectionValue = theme.ColorSelectionValue;
            }
        }

        private async void UpdateThemeConfig()
        {
            var themePreference = await _lemonDbContext.UserPreference.FirstOrDefaultAsync(x => x.Id == LemonConstants.ThemeConfigId);
            if (themePreference == null) return;
            var newTheme = new ThemeConfig
            {
                IsDarkTheme = IsDarkTheme,
                IsColorAdjusted = IsColorAdjusted,
                DesiredContrastRatio = DesiredContrastRatio,
                ContrastValue = ContrastValue,
                ColorSelectionValue = ColorSelectionValue,
            };

            themePreference.Content = JsonSerializer.Serialize(newTheme);
            themePreference.LastModifiedAt = DateTime.Now;
            await _lemonDbContext.SaveChangesAsync();
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