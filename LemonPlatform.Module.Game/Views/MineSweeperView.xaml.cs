﻿using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Game.Views
{
    public partial class MineSweeperView : Page, ITransientDependency
    {
        public MineSweeperView()
        {
            InitializeComponent();
        }
    }
}