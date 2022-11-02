using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace Minesweeper.Uno.Converters
{
    internal class ValueTupleToMargin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var margin=(((double, double, double, double))value);
            return new Thickness(margin.Item1, margin.Item2, margin.Item3, margin.Item4);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
