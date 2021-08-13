using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace MyMusic.Wpf.Converters
{
    /// <summary>
    /// Converts text to rich text to highlight the search term.
    /// Got inspiration from https://stackoverflow.com/questions/33315611
    /// </summary>
    /// <seealso cref="System.Windows.Data.IMultiValueConverter" />
    public class SearchTextToRichTextConverter : IMultiValueConverter
    {
        string[] searchTokens = new string[] {
            "artist:",
            "album:",
            "title:"
        };

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return null;
            string text = System.Convert.ToString(values[0]);
            string searchText = System.Convert.ToString(values[1]);
            if (string.IsNullOrEmpty(text)) return null;
            List<Inline> inlines = new List<Inline>();
            if (string.IsNullOrEmpty(searchText))
            {
                inlines.Add(new Run(text));
                return inlines;
            }
            string highlightText = searchText;
            string searchToken = searchTokens.FirstOrDefault(i => searchText.StartsWith(i));
            if(!string.IsNullOrEmpty(searchToken))
            {
                highlightText = searchText.Substring(searchToken.Length).Trim();
                if (string.IsNullOrEmpty(highlightText))
                {
                    inlines.Add(new Run(text));
                    return inlines;
                }
            }
           
            int index = text.IndexOf(highlightText, StringComparison.CurrentCultureIgnoreCase);
            if (index < 0)
            {
                inlines.Add(new Run(text));
                return inlines;
            }

            Brush selectionColor = Application.Current.Resources["SearhHighlightBrush"] as SolidColorBrush;
            Brush forecolor = Application.Current.Resources["SearchHighlightForeground"] as SolidColorBrush;

            inlines.Clear();
            while (true)
            {
                inlines.AddRange(new Inline[] {
                        new Run(text.Substring(0, index)),
                        new Run(text.Substring(index, highlightText.Length)) {Background = selectionColor,
                            Foreground = forecolor}
                    });

                text = text.Substring(index + highlightText.Length);
                index = text.IndexOf(highlightText, StringComparison.CurrentCultureIgnoreCase);

                if (index < 0)
                {
                    inlines.Add(new Run(text));
                    break;
                }
            }
            return inlines;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
