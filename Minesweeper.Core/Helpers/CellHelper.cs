namespace Minesweeper.Core.Helpers
{
    public class CellHelper
    {
        public static (int x,int y) GetPosFromIndex(int index,int columns)
        {
            int y = (int)Math.Floor(((double)index) / columns);
            int x = (int)Math.Floor(((double)index) % columns);
            return (x, y);
        }
        public static int GetIndexFromPos(int x,int y,int rows,int columns)
        {
            return y * columns + x;
        }
        public static List<(int x,int y)> GetNearbyCells(int row,int column,int columns,int rows)
        {
            List<(int x,int y)> cells = new List<(int x, int y)>();
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    int nearByCellRow = row + y;
                    int nearByCellColumn = column + x;
                    if (x == 0 && y == 0)
                        continue;
                    if (nearByCellColumn >= 0 && nearByCellColumn < columns && nearByCellRow >= 0 && nearByCellRow < rows)
                    {
                        cells.Add((nearByCellColumn,nearByCellRow));
                    }
                }
            }
            return cells;
        }
    }
}
