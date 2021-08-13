using MyMusic.Wpf.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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




        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(TileViewList));



        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(TileViewList));



    }
}
