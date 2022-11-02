using Microsoft.UI.Xaml.Data;
using Minesweeper.Core.Models;
using System;

namespace Minesweeper.Uno.Converters
{
    internal class GameDifficultyToBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var target = (GameDifficulty.Difficulty)int.Parse(parameter as string);
            var current = ((GameDifficulty)value).DifficultyType;
            return target == current;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
