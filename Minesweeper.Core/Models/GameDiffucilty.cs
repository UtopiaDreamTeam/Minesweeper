namespace Minesweeper.Core.Models
{
    public class GameDifficulty
    {
        private int rows;
        private int columns;
        private int boombsCount;

        public enum Difficulty
        {
            Easy, Meduim, Hard, Custom
        }
        public Difficulty DifficultyType { get; set; }
        public int Columns { 
            get => columns; 
            set => columns=Math.Max(8,Math.Min(30, value));
        }
        public int Rows
        {
            get => rows;
            set => rows=Math.Max(8,Math.Min(24,value));
        }
        public int BoombsCount { 
            get => boombsCount;
            set => boombsCount = Math.Min((Rows-1)*(Columns-1), Math.Max(10, value)); 
        }

        public GameDifficulty(int rows, int columns, int boombsCount)
        {
            Columns = columns;
            Rows = rows;
            BoombsCount = boombsCount;
            DifficultyType = Difficulty.Custom;
        }
        private GameDifficulty(int rows, int columns, int boombsCount, Difficulty difficulty)
        {
            Columns = columns;
            Rows = rows;
            BoombsCount = boombsCount;
            DifficultyType = difficulty;
        }
        public static GameDifficulty EasyDifficulty => new GameDifficulty(8, 8, 10, Difficulty.Easy);
        public static GameDifficulty MeduimDifficulty => new GameDifficulty(16, 16, 40, Difficulty.Meduim);
        public static GameDifficulty HardDifficulty => new GameDifficulty(16, 30, 99, Difficulty.Hard);
    }
}
