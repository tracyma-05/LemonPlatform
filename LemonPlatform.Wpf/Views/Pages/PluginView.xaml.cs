﻿using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.ViewModels.Pages;
using System.Windows.Controls;

namespace LemonPlatform.Wpf.Views.Pages
{
    public partial class PluginView : UserControl, ITransientDependency
    {
        public PluginView(PluginViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}