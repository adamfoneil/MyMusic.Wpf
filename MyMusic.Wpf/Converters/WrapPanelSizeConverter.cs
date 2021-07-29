using System;
using System.Globalization;
using System.Windows.Data;

namespace MyMusic.Wpf.Converters
{
    /// <summary>
    /// This converter class is used to adjust the wrap panel width in TileViewList.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class WrapPanelSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val && !double.IsNaN(val))
            {
                return val - 20.0d;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
