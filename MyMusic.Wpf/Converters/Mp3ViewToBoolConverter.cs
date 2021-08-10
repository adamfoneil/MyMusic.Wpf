using MyMusic.Wpf.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MyMusic.Wpf.Converters
{
    /// <summary>
    /// Mp3 view option to boolean converter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class Mp3ViewToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Mp3ViewOption mp3ViewOption && parameter is string param)
            {
                switch (mp3ViewOption)
                {
                    case Mp3ViewOption.ListView:
                        return param == "List";
                    case Mp3ViewOption.ArtistView:
                        return param == "Artist";
                    case Mp3ViewOption.AlbumView:
                        return param == "Album";                    
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool val && parameter is string param)
            {
                switch (param)
                {
                    case "List":
                        return Mp3ViewOption.ListView;
                    case "Artist":
                        return Mp3ViewOption.ArtistView;
                    case "AlbumView":
                        return Mp3ViewOption.AlbumView;
                }
            }
            return Mp3ViewOption.ListView;
        }
    }
}
