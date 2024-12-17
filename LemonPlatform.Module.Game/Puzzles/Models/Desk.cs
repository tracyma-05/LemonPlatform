using CommunityToolkit.Mvvm.ComponentModel;
using LemonPlatform.Module.Game.Puzzles.Core.Figures;
using System.Collections.ObjectModel;

namespace LemonPlatform.Module.Game.Puzzles.Models
{
    [ObservableObject]
    public partial class Desk
    {
        [ObservableProperty]
        private ObservableCollection<DeskModel> _deskItems;

        [ObservableProperty]
        private int _index;

        public FigurePlus[] AllKinds { get; set; }

        public int KindIndex { get; set; } = 0;
    }
}