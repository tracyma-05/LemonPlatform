﻿using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Json.ViewModels;
using System.Windows.Controls;

namespace LemonPlatform.Module.Json.Views
{
    public partial class JsonView : Page, ITransientDependency
    {
        public JsonView(JsonViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}