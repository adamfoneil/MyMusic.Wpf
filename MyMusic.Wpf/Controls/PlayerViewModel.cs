using MyMusic.Wpf.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Media;

namespace MyMusic.Wpf.Controls
{
    public class PlayerViewModel : BindableBase
    {
        private MediaPlayer _player;
        private readonly ObservableCollection<Mp3File> _playlist;
        private readonly PlayHistory _history;

        private int _track;

        public PlayerViewModel(PlayHistory playHistory)
        {
            _history = playHistory;

            _player = new MediaPlayer();
            _player.MediaEnded += player_MediaEnded;

            _playlist = new ObservableCollection<Mp3File>();
            _playlist.CollectionChanged += PlaylistChanged;

            PlayNextCommand = new DelegateCommand(PlayNextTrack, () =>
            {
                int currentTrackIndex = _playlist.IndexOf(CurrentTrack);
                return currentTrackIndex > -1 && currentTrackIndex + 1 < _playlist.Count;
            });

            PlayPreviousCommand = new DelegateCommand(PlayPreviousTrack, () =>
            {
                int currentTrackIndex = _playlist.IndexOf(CurrentTrack);
                return currentTrackIndex > -1 && currentTrackIndex > 0;
            });
        }

        /// <summary>
        /// Gets or sets the play next command.
        /// </summary>
        /// <value>
        /// The play next command.
        /// </value>
        public DelegateCommand PlayNextCommand { get; set; }

        /// <summary>
        /// Gets or sets the play previous command.
        /// </summary>
        /// <value>
        /// The play previous command.
        /// </value>
        public DelegateCommand PlayPreviousCommand { get; set; }

        private void PlayPreviousTrack()
        {
            int currentTrackIndex = _playlist.IndexOf(CurrentTrack);
            CurrentTrack = _playlist[currentTrackIndex - 1];
        }

        private void PlayNextTrack()
        {
            int currentTrackIndex = _playlist.IndexOf(CurrentTrack);
            CurrentTrack = _playlist[currentTrackIndex + 1];
        }

        private void PlaylistChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_playlist.Count == 1)
            {
                CurrentTrack = _playlist[0];
            }
            PlayNextCommand.RaiseCanExecuteChanged();
            PlayPreviousCommand.RaiseCanExecuteChanged();
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
            if (!_playlist.Contains(file))
            {
                _playlist.Add(file);
                _track = _playlist.Count;
            }
            else
            {
                CurrentTrack = file;
                _track = _playlist.IndexOf(file);
            }
        }

        public void PlayNext(Mp3File file)
        {
            try
            {
                _playlist.Insert(_track + 1, file);
            }
            catch
            {
                PlayAtEnd(file);
            }
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
                        _track = _playlist.IndexOf(value);
                        _player.Open(new Uri($"file://{value.FullPath}"));
                        _player.Play();
                        _history.Add(value.FullPath);
                    }
                    else
                    {
                        _player.Stop();
                    }
                    PlayNextCommand.RaiseCanExecuteChanged();
                    PlayPreviousCommand.RaiseCanExecuteChanged();
                    SetProperty(ref _file, value);
                }
            }
        }


        public MediaPlayer MediaPlayer
        {
            get { return _player; }
            set
            {
                SetProperty(ref _player, value);
            }
        }

        public void PlayCategory(IEnumerable<Mp3File> mp3Files)
        {
            _playlist.Clear();
            foreach (var item in mp3Files)
            {
                _playlist.Add(item);
            }
        }
    }
}
