using Minesweeper.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core.ViewModels
{
    public class ScorePageViewModel
    {
        public ScorePageViewModel(Dictionary<GameDifficulty.Difficulty, (string, int)> scores)
        {
            Scores = scores;
        }

        public Dictionary<GameDifficulty.Difficulty, (string, int)> Scores { get; }
    }
}
