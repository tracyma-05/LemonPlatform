﻿using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.ViewModels.Pages;
using System.Windows.Controls;

namespace LemonPlatform.Wpf.Views.Pages
{
    public partial class ChatView : UserControl, ISingletonDependency
    {
        public ChatView(ChatViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}