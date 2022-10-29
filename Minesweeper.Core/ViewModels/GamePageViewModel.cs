using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minesweeper.Core.Helpers;
using Minesweeper.Core.Interfaces;
using Minesweeper.Core.Models;
using Minesweeper.WinUI.Models;
using System.Diagnostics;

namespace Minesweeper.Core.ViewModels
{
    public class GamePageViewModel : ObservableObject
    {

        private Cell[]? cells;
        public Cell[] Cells { 
            get => cells!; 
            private set =>SetProperty(ref cells,value); 
        }

        private GameStat gameStat=GameStat.Idle;
        public GameStat GameStat
        {
            get => gameStat;
            private set => SetProperty(ref gameStat, value);
        }
        private int flagsUsed;
        public int FlagsUsed
        {
            get => flagsUsed;
            set => SetProperty(ref flagsUsed, value);

        }
        private int time;
        public int Time { 
            get => time; 
            set => SetProperty(ref time, Math.Min(999,value)); 
        }

        public Settings Settings { get; }

        private int solved;
        private int flagsRight;
        private readonly List<(int, int)> bombs = new List<(int, int)>();
        private readonly System.Timers.Timer timer=new System.Timers.Timer(800);
        private readonly Stopwatch stopwatch=new Stopwatch();
        private IAskForName askForName;

        public GamePageViewModel(IRunOnMainThread runOnMainThread,IAskForName askForName)
        {
            this.askForName=askForName;
            Settings =Settings.Load();
            RestartCommand = new RelayCommand(StartGame);
            GoEasyCommand = new RelayCommand(() => ChangeDifficulty(GameDifficulty.EasyDifficulty));
            GoMeduimCommand = new RelayCommand(() => ChangeDifficulty(GameDifficulty.MeduimDifficulty));
            GoHardCommand = new RelayCommand(() => ChangeDifficulty(GameDifficulty.HardDifficulty));
            void ChangeDifficulty(GameDifficulty difficulty)
            {
                Settings.GameDifficulty = difficulty;
                RestartCommand.Execute(null);
            }
            timer.AutoReset = true;
            timer.Elapsed += (s, args) =>
            {
                runOnMainThread.Run(() => Time = (int)Math.Ceiling(stopwatch.Elapsed.TotalSeconds));
            };
            StartGame();
        }
        private void StartGame()
        {
            stopwatch.Stop();
            timer.Stop();
            Time = 0;
            cells = new Cell[Settings.GameDifficulty.Columns * Settings.GameDifficulty.Rows];
            GameStat = GameStat.Playing;
            solved = 0;
            FlagsUsed = Settings.GameDifficulty.BoombsCount;
            flagsRight = 0;
            for (int i = 0; i < Settings.GameDifficulty.Rows*Settings.GameDifficulty.Columns; i++)
            {
                Cells[i]=new Cell(OnLeftClick,OnRightClick, i);
            }
            OnPropertyChanged(nameof(Cells));
            GenerateBoombs();
        }
        private void GenerateBoombs()
        {
            bombs.Clear();
            for (int i = 0; i < Settings.GameDifficulty.BoombsCount; i++)
            {
                GenerateBoomb();
            }
            void GenerateBoomb()
            {
                int row = Random.Shared.Next(0,Settings.GameDifficulty.Rows);
                int column = Random.Shared.Next(0, Settings.GameDifficulty.Columns);
                if (bombs.Contains((column, row)))
                    GenerateBoomb();
                else bombs.Add((column, row));
            }
        }

        private async void OnLeftClick(int index)
        {
            if (GameStat!=GameStat.Playing)
                return;
            if (!stopwatch.IsRunning)
            {
                stopwatch.Restart();
                timer.Start();
            }
            var (x, y) = CellHelper.GetPosFromIndex(index, Settings.GameDifficulty.Columns);
            if (bombs.Contains((x, y)))
            {
                GameStat = GameStat.Lost;
                stopwatch.Stop();
                timer.Stop();
                OnGameOver();
            }
            else
            {
                OnSafeSpot(Cells[index], x, y);
                await CheckForWin();
            }
        }
        private void OnSafeSpot(Cell btn, int x, int y)
        {
            if (!btn.IsEnabled)
                return;
            int boombsCount = GetNearbyBoombsCount(x, y);
            if (bombs.Contains((x, y)))
                return;
            btn.Display=new CellDisplay(boombsCount);
            btn.IsEnabled = false;
            solved++;
            if (boombsCount == 0)
            {
                foreach (var cell in CellHelper.GetNearbyCells(y, x, Settings.GameDifficulty.Columns, Settings.GameDifficulty.Rows))
                {
                    var index = CellHelper.GetIndexFromPos(cell.x, cell.y, Settings.GameDifficulty.Rows, Settings.GameDifficulty.Columns);
                    OnSafeSpot(Cells[index], cell.x, cell.y);
                }
            }

        }
        private int GetNearbyBoombsCount(int column, int row)
        {
            int count = 0;
            foreach (var (x, y) in CellHelper.GetNearbyCells(row, column, Settings.GameDifficulty.Columns, Settings.GameDifficulty.Rows))
            {
                if (bombs.Contains((x, y)))
                    count++;
            }
            return count;
        }

        private async void OnRightClick(int index)
        {
            if (gameStat!=GameStat.Playing)
                return;
            if (Cells[index].Display.DisplayType == DisplayType.Flag)
            {
                if (bombs.Contains(CellHelper.GetPosFromIndex(index, Settings.GameDifficulty.Columns)))
                    flagsRight--;
                if (Settings.Marks)
                    Cells[index].Display = new CellDisplay(DisplayType.Unknown);
                else Cells[index].Display = new CellDisplay(DisplayType.Empty);
                FlagsUsed++;
            }
            else if (Cells[index].Display.DisplayType == DisplayType.Unknown)
            {
                Cells[index].Display = new CellDisplay(DisplayType.Empty);
            }
            else
            {
                if (bombs.Contains(CellHelper.GetPosFromIndex(index, Settings.GameDifficulty.Columns)))
                    flagsRight++;
                FlagsUsed--;
                Cells[index].Display = new CellDisplay(DisplayType.Flag);
                await CheckForWin();
            }
        }
        private async Task CheckForWin()
        {
            if (solved + Settings.GameDifficulty.BoombsCount == Settings.GameDifficulty.Columns * Settings.GameDifficulty.Rows || flagsRight == Settings.GameDifficulty.BoombsCount)
            {
                GameStat= GameStat.Won;
                stopwatch.Stop();
                timer.Stop();
                if (Settings.HighScores.TryGetValue(Settings.GameDifficulty.DifficultyType,out (string,int)oldHighScore)&&oldHighScore.Item2>Time)
                {
                    Settings.HighScores[Settings.GameDifficulty.DifficultyType] = (await askForName.AskForName(), Time);
                    await Settings.Save();
                }
                OnGameOver();
            }
        }
        private void OnGameOver()
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cell? cell = Cells[i];
                if (bombs.Contains(CellHelper.GetPosFromIndex(i, Settings.GameDifficulty.Columns)))
                {
                    if (cell.Display.DisplayType != DisplayType.Flag)
                        cell.Display = new CellDisplay(DisplayType.Bomb);
                }
                else if (cell.Display.DisplayType == DisplayType.Flag)
                    cell.Display = new CellDisplay(DisplayType.Wrong); 
            }
        }

        public RelayCommand RestartCommand { get; }
        public RelayCommand GoEasyCommand { get; }
        public RelayCommand GoMeduimCommand { get; }
        public RelayCommand GoHardCommand { get; }

    }
}
