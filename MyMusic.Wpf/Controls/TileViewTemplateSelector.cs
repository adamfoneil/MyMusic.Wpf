using MyMusic.Wpf.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyMusic.Wpf.Controls
{
    public class TileViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ArtistTemplate { get; set; }

        public DataTemplate AlbumTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                if (item.ToString() == "{NewItemPlaceholder}")
                    return base.SelectTemplate(item, container);

                return item switch
                {
                    AlbumGroup _ => AlbumTemplate,
                    ArtistGroup _ => ArtistTemplate,
                    Mp3File _ => base.SelectTemplate(item, container),
                    _ => throw new NotImplementedException($"Datatemplate is not implemented for {item.GetType()} control type in TileViewList.xaml"),
                };
            }
            return base.SelectTemplate(item, container);
        }
    }
}
