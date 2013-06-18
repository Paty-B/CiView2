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

namespace CiView.Recorder
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        LogPlayer _player;
        public UserControl1()
        {
            InitializeComponent();
            _player = null;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayButton.IsEnabled = false;
            OpenButton.IsEnabled = false;
            DisposeButton.IsEnabled = false;
            if ((bool)OnTimeCheckBox.IsChecked)
            {
                while (_player.PlayOnTime() == 0) ;
            }
            else
            {
                while (_player.Play() == 0) ;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            OpenButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            PlayButton.IsEnabled = true;
            DisposeButton.IsEnabled = true;
            _player.Stop();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog FileDialog = new Microsoft.Win32.OpenFileDialog();
            FileDialog.ShowDialog();
            if (FileDialog.FileName != string.Empty)
            {
                if(_player!=null) _player.Dispose();
                _player = new LogPlayer(FileDialog.FileName);
                PlayButton.IsEnabled = true;
                OnTimeCheckBox.IsEnabled = true;
                DisposeButton.IsEnabled = true;

            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseButton.IsEnabled = false;
            PlayButton.IsEnabled = true;
            _player.Pause();
        }

        private void DisposeButton_Click(object sender, RoutedEventArgs e)
        {
            _player.Dispose();
            DisposeButton.IsEnabled = false;
            PauseButton.IsEnabled = false;
            PlayButton.IsEnabled = false;
            StopButton.IsEnabled = false;
            OpenButton.IsEnabled = true;
        }
    }
}
