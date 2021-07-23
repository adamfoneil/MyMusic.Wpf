using MyMusic.Wpf.Models;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MyMusic.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for PlayerControls.xaml
    /// </summary>
    public partial class PlayerControls : UserControl
    {
        private bool _isPlaying = false;
        private bool _isDragging = false;
        DispatcherTimer _timer;
        public PlayerControls()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
            LayoutRoot.DataContext = this;
            PlayPauseCommand = new DelegateCommand(PlayButton_Click);
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (!_isDragging)
            {
                seekBar.Value = MediaPlayer.Position.TotalSeconds;
                runningTimeLabel.Text = $"{MediaPlayer.Position:mm\\:ss}";
            }
        }

        private void PlayButton_Click()
        {
            if (IsPlaying)
            {
                MediaPlayer.Pause();
                IsPlaying = false;
            }
            else
            {
                MediaPlayer.Play();
                IsPlaying = true;
            }
        }

        public MediaPlayer MediaPlayer
        {
            get { return (MediaPlayer)GetValue(MediaPlayerProperty); }
            set { SetValue(MediaPlayerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MediaPlayer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MediaPlayerProperty =
            DependencyProperty.Register("MediaPlayer", typeof(MediaPlayer), typeof(PlayerControls), new PropertyMetadata(null, (sender, args) =>
            {
                if (sender is PlayerControls playerControls && args.NewValue != null)
                {
                    playerControls.MediaPlayer.MediaOpened -= playerControls.MediaPlayer_MediaOpened;
                    playerControls.MediaPlayer.MediaOpened += playerControls.MediaPlayer_MediaOpened;
                    playerControls.MediaPlayer.MediaEnded -= playerControls.MediaPlayer_MediaEnded;
                    playerControls.MediaPlayer.MediaEnded += playerControls.MediaPlayer_MediaEnded;
                }
            }));

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            IsPlaying = false;
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            IsPlaying = true;
            if (MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = MediaPlayer.NaturalDuration.TimeSpan;
                totalDurationLabel.Text = $"{ts:mm\\:ss}";
                seekBar.Minimum = 0;
                seekBar.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                seekBar.SmallChange = 1;
                seekBar.LargeChange = Math.Min(10, ts.Seconds / 10);
            }
            _timer.Start();
        }

        internal bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                if (_isPlaying)
                {
                    playButton.Visibility = Visibility.Collapsed;
                    pauseButton.Visibility = Visibility.Visible;
                }
                else
                {
                    playButton.Visibility = Visibility.Visible;
                    pauseButton.Visibility = Visibility.Collapsed;
                }
            }
        }


        public Mp3File CurrentTrack
        {
            get { return (Mp3File)GetValue(CurrentTrackProperty); }
            set { SetValue(CurrentTrackProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTrackProperty =
            DependencyProperty.Register("CurrentTrack", typeof(Mp3File), typeof(PlayerControls), new PropertyMetadata(null, (sender, args) =>
            {
                if (sender is PlayerControls playerControls)
                {
                    playerControls.playButton.IsEnabled = true;
                    playerControls.pauseButton.IsEnabled = true;
                }
            }));


        public ICommand PlayNextCommand
        {
            get { return (ICommand)GetValue(PlayNextCommandProperty); }
            set { SetValue(PlayNextCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayNextCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayNextCommandProperty =
            DependencyProperty.Register("PlayNextCommand", typeof(ICommand), typeof(PlayerControls), new PropertyMetadata(null));


        public ICommand PlayPreviousCommand
        {
            get { return (ICommand)GetValue(PlayPreviousCommandProperty); }
            set { SetValue(PlayPreviousCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayPreviousCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayPreviousCommandProperty =
            DependencyProperty.Register("PlayPreviousCommand", typeof(ICommand), typeof(PlayerControls), new PropertyMetadata(null));


        internal ICommand PlayPauseCommand
        {
            get { return (ICommand)GetValue(PlayPauseCommandProperty); }
            set { SetValue(PlayPauseCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayPauseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayPauseCommandProperty =
            DependencyProperty.Register("PlayPauseCommand", typeof(ICommand), typeof(PlayerControls), new PropertyMetadata(null));

        private void seekBar_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _isDragging = true;
        }

        private void seekBar_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _isDragging = false;
            MediaPlayer.Position = TimeSpan.FromSeconds(seekBar.Value);
        }
    }
}
