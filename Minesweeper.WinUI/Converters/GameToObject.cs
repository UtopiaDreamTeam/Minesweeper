using Microsoft.UI.Xaml.Data;
using Minesweeper.Core.Models;
using System;

namespace Minesweeper.WinUI.Converters
{
    internal class GameToObject : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var game= (GameStat)value;
            return game switch
            {
                GameStat.Won => "You Win",
                GameStat.Lost=>"Loser",
                GameStat.Playing=>"Restart",
                _=>null,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
