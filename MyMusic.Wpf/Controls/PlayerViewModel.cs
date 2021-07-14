﻿using MyMusic.Wpf.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyMusic.Wpf.Controls
{
    public class PlayerViewModel : BindableBase
    {
        private readonly MediaPlayer _player;
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
                    
                    SetProperty(ref _file, value);
                }
            }
        }
    }
}
