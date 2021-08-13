using MyMusic.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MyMusic.Wpf.Converters
{
    /// <summary>
    /// Mp3 view option to tile/list view visilibity converter.
    /// </summary>
    public class Mp3ViewToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Mp3ViewOption viewOption && parameter is string param)
            {
                switch (viewOption)
                {
                    case Mp3ViewOption.ListView:
                        return param == "ListView" ? Visibility.Visible : Visibility.Collapsed;
                    case Mp3ViewOption.ArtistView:                        
                    case Mp3ViewOption.AlbumView:
                        return param == "TileView" ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
