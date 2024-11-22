﻿using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.ViewModels.Pages;
using System.Windows.Controls;

namespace LemonPlatform.Wpf.Views.Pages
{
    public partial class LogView : Page, ITransientDependency
    {
        public LogView(LogViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}