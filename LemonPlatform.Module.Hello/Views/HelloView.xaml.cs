﻿using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Hello.ViewModels;
using System.Windows.Controls;

namespace LemonPlatform.Module.Hello.Views
{
    public partial class HelloView : Page, ITransientDependency
    {
        public HelloView(HelloViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}