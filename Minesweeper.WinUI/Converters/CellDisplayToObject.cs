using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Minesweeper.Core.Models;
using System;

namespace Minesweeper.WinUI.Converters
{
    internal class CellDisplayToObject : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var display = (CellDisplay)value;
            return display.DisplayType switch
            {
                DisplayType.Number => new TextBlock() { Text=display.Number.ToString()},
                DisplayType.Flag => new SymbolIcon(Symbol.Flag),
                DisplayType.Bomb => new SymbolIcon(Symbol.Emoji),
                DisplayType.Unknown => new SymbolIcon(Symbol.Help),
                DisplayType.Wrong => new SymbolIcon(Symbol.Clear),
                _ => null,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
