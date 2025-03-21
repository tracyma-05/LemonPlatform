﻿using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.ViewModels.Pages;
using System.Windows.Controls;

namespace LemonPlatform.Wpf.Views.Pages
{
    public partial class SettingView : UserControl, ISingletonDependency
    {
        public SettingView(SettingViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}