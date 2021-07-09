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
        public IEnumerable<Mp3File> Mp3Files
        {
            get { return (IEnumerable<Mp3File>)GetValue(Mp3FilesProperty); }
            set { SetValue(Mp3FilesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Mp3FilesProperty =
            DependencyProperty.Register("Mp3Files", typeof(IEnumerable<Mp3File>), typeof(Playlist), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the current track.
        /// </summary>
        /// <value>
        /// The current track.
        /// </value>
        public Mp3File CurrentTrack
        {
            get { return (Mp3File)GetValue(CurrentTrackProperty); }
            set { SetValue(CurrentTrackProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTrack.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTrackProperty =
            DependencyProperty.Register("CurrentTrack", typeof(Mp3File), typeof(Playlist), new PropertyMetadata(null));


        // When the user double clicks on the data grid, this will further triggers the Mp3FileSelected event to notify its subscriber.
        private void dgFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            var mp3file = grid.CurrentItem as Mp3File;
            Mp3FileSelected?.Invoke(mp3file);
        }
    }
}
