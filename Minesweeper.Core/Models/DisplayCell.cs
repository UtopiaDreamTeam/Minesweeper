namespace Minesweeper.WinUI.Models
{
    public struct CellDisplay
    {
        public CellDisplay(DisplayType displayType)
        {
            DisplayType = displayType;
        }
        public CellDisplay(int number)
        {
            if(number < 0||number>8)
                throw new ArgumentOutOfRangeException(nameof(number));
            if (number != 0)
                DisplayType = DisplayType.Number;
            Number = number;
        }

        public int Number { get; } = 0;
        public DisplayType DisplayType { get; }=DisplayType.Empty;

    }
    public enum DisplayType
    {
        Number, Flag, Unknown, Bomb, Empty,Wrong
    }
}
