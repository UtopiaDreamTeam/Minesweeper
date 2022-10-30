using Minesweeper.Core.Models;

namespace Minesweeper.Core.ViewModels
{
    public class CustomFieldPageViewModel
    {
        public GameDifficulty GameDifficulty { get; set; }
        public CustomFieldPageViewModel(GamePageViewModel gamePageView)
        {
            var currentDiffculty = gamePageView.Settings.GameDifficulty;
            GameDifficulty=new GameDifficulty(currentDiffculty.Rows,currentDiffculty.Columns,currentDiffculty.BombsCount);
        }
    }
}
