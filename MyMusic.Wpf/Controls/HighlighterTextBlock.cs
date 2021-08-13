using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MyMusic.Wpf.Controls
{
    /// <summary>
    /// Got inspiration from https://stackoverflow.com/questions/33315611
    /// </summary>
    public class HighlighterTextBlock : TextBlock
    {
        public List<Inline> RichText
        {
            get { return (List<Inline>)GetValue(RichTextProperty); }
            set { SetValue(RichTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RichText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RichTextProperty =
            DependencyProperty.Register("RichText", typeof(List<Inline>), typeof(HighlighterTextBlock), new PropertyMetadata(null, OnInlineChanged));

        public static void OnInlineChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            HighlighterTextBlock r = sender as HighlighterTextBlock;
            List<Inline> i = e.NewValue as List<Inline>;
            if (r == null || i == null)
                return;
            r.Inlines.Clear();
            foreach (Inline inline in i)
            {
                r.Inlines.Add(inline);
            }
        }       
    }
}
