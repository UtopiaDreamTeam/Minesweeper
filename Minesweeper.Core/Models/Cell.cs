using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minesweeper.WinUI.Models;

namespace Minesweeper.Core.Models
{
    public class Cell : ObservableObject
    {

        private double height;
        public double Height { get => height; set => SetProperty(ref height, value); }


        private double width;
        public double Width { get => width; set => SetProperty(ref width, value); }


        private (double, double, double, double) margin;
        public (double, double, double, double) Margin { get => margin; set => SetProperty(ref margin, value); }


        private CellDisplay display = new CellDisplay(DisplayType.Empty);
        public CellDisplay Display {get=> display;set => SetProperty(ref display, value);}


        private bool isEnabled = true;
        public bool IsEnabled { get => isEnabled; set => SetProperty(ref isEnabled, value); }


        public RelayCommand LeftClickCommand { get; }
        public RelayCommand RightClickCommand { get; }



        public Cell(Action<int> onLeftClick, Action<int> onRightClick, int index)
        {
            LeftClickCommand = new RelayCommand(() => onLeftClick.Invoke(index));
            RightClickCommand = new RelayCommand(() => onRightClick.Invoke(index));
        }

    }
    public enum CellDisplayType
    {
        Empty,Number,Bomb,Flag,Unknown
    }
}
