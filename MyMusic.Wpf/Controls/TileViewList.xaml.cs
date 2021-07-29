using MyMusic.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyMusic.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for TileViewList.xaml
    /// </summary>
    public partial class TileViewList : UserControl
    {
        public TileViewList()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the list of MP3 files.
        /// </summary>
        /// <value>
        /// The MP3 files.
        /// </value>
        public object Mp3Files
        {
            get { return (object)GetValue(Mp3FilesProperty); }
            set { SetValue(Mp3FilesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Mp3FilesProperty =
            DependencyProperty.Register("Mp3Files", typeof(object), typeof(TileViewList), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the current track.
        /// </summary>
        /// <value>
        /// The current track.
        /// </value>
        public object CurrentTrack
        {
            get { return (Mp3File)GetValue(CurrentTrackProperty); }
            set { SetValue(CurrentTrackProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTrack.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTrackProperty =
            DependencyProperty.Register("CurrentTrack", typeof(object), typeof(TileViewList), new PropertyMetadata(null));



        public ICommand PlayCommand
        {
            get { return (ICommand)GetValue(PlayCommandProperty); }
            set { SetValue(PlayCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayCommandProperty =
            DependencyProperty.Register("PlayCommand", typeof(ICommand), typeof(TileViewList), new PropertyMetadata(null));


    }
}
