using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
using MyMusic.Wpf.Static;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyMusic.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for Playlist.xaml
    /// </summary>
    public partial class Playlist : UserControl
    {

        public Playlist()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        /// <summary>
        /// Occurs when [MP3 file selected].
        /// </summary>
        public event Action<Mp3File> Mp3FileSelected;

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
            DependencyProperty.Register("Mp3Files", typeof(object), typeof(Playlist), new PropertyMetadata(null));



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
            DependencyProperty.Register("CurrentTrack", typeof(object), typeof(Playlist), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets a value indicating whether [enable context menu].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable context menu]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableContextMenu
        {
            get { return (bool)GetValue(EnableContextMenuProperty); }
            set { SetValue(EnableContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableContextMenuProperty =
            DependencyProperty.Register("EnableContextMenu", typeof(bool), typeof(Playlist), new PropertyMetadata(false));



        /// <summary>
        /// Gets or sets the play next command.
        /// </summary>
        /// <value>
        /// The play next command.
        /// </value>
        public ICommand PlayNextCommand
        {
            get { return (ICommand)GetValue(PlayNextCommandProperty); }
            set { SetValue(PlayNextCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayNextCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayNextCommandProperty =
            DependencyProperty.Register("PlayNextCommand", typeof(ICommand), typeof(Playlist));



        /// <summary>
        /// Gets or sets the play at end command.
        /// </summary>
        /// <value>
        /// The play at end command.
        /// </value>
        public ICommand PlayAtEndCommand
        {
            get { return (ICommand)GetValue(PlayAtEndCommandProperty); }
            set { SetValue(PlayAtEndCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayAtEndCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayAtEndCommandProperty =
            DependencyProperty.Register("PlayAtEndCommand", typeof(ICommand), typeof(Playlist));



        public ICommand RescanCommand
        {
            get { return (ICommand)GetValue(RescanCommandProperty); }
            set { SetValue(RescanCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RescanCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RescanCommandProperty =
            DependencyProperty.Register("RescanCommand", typeof(ICommand), typeof(Playlist));




        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(Playlist));



        // When the user double clicks on the data grid, this will further triggers the Mp3FileSelected event to notify its subscriber.
        private void dgFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            var mp3file = grid.CurrentItem as Mp3File;
            Mp3FileSelected?.Invoke(mp3file);
        }

        private void viewFileLocation_Click(object sender, RoutedEventArgs e) => FileUtil.RevealInExplorer((CurrentTrack as Mp3File).FullPath);
    }
}
