﻿using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Tools.Views
{
    public partial class JsonExtractToolView : Page, ISingletonDependency
    {
        public JsonExtractToolView()
        {
            InitializeComponent();
        }
    }
}