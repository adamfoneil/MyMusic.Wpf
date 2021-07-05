using MyMusic.Wpf.Models;
using System;
using System.ComponentModel;
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

        public Player()
        {
            InitializeComponent();
            _player = new MediaPlayer();
        }

        private Mp3File _file;
       
        public Mp3File Mp3File
        {
            get => _file;
            set
            {
                if (value != _file)
                {                    
                    _player.Open(new Uri($"file://{value.FullPath}"));                    
                    _file = value;
                    DataContext = _file;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mp3File)));
                    _player.Play();
                }
            }
        }
    }
}
