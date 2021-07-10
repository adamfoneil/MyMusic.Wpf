using MyMusic.Wpf.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyMusic.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly MediaPlayer _player;
        private readonly ObservableCollection<Mp3File> _playlist;
        private readonly PlayHistory _history;

        private int _track;

        public Player()
        {
            //_history = history; how can this be set with dependency injection? or is this not the right way?

            InitializeComponent();
            DataContext = this;

            _player = new MediaPlayer();
            _player.MediaEnded += player_MediaEnded;

            _playlist = new ObservableCollection<Mp3File>();
            _playlist.CollectionChanged += PlaylistChanged;
        }

        private void PlaylistChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_playlist.Count == 1)
            {
                CurrentTrack = _playlist[0];
            }
        }

        private void player_MediaEnded(object sender, EventArgs e)
        {
            _track++;

            if (_track > _playlist.Count - 1)
            {
                _player.Stop();
                return;
            }

            CurrentTrack = _playlist[_track];
        }

        public ObservableCollection<Mp3File> Playlist => _playlist;

        public void PlayNow(Mp3File file)
        {
            _track = 0;
            if (!_playlist.Contains(file))
                _playlist.Insert(0, file);
            else
            {
                CurrentTrack = file;
            }
        }

        public void PlayNext(Mp3File file)
        {
            _playlist.Insert(_playlist.Count > 0 ? 1 : 0, file);
        }

        public void PlayAtEnd(Mp3File file)
        {
            _playlist.Add(file);
        }

        private Mp3File _file;

        public Mp3File CurrentTrack
        {
            get => _file;
            private set
            {
                if (value != _file)
                {
                    if (File.Exists(value?.FullPath))
                    {
                        _player.Open(new Uri($"file://{value.FullPath}"));
                        _player.Play();
                    }
                    else
                    {
                        _player.Stop();
                    }

                    _file = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTrack)));
                }
            }
        }

        private void playlistControl_Mp3FileSelected(Mp3File mp3file)
        {
            PlayNow(mp3file);
        }
    }
}
