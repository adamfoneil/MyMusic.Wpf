using MyMusic.Wpf.Models;
using System.Windows.Controls;

namespace MyMusic.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {

        public Player()
        {
            InitializeComponent();
        }

        private void playlistControl_Mp3FileSelected(Mp3File mp3file)
        {
            if (DataContext is PlayerViewModel playerVM)
            {
                playerVM.PlayNow(mp3file);
            }

        }
    }
}
