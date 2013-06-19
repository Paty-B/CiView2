using Randomizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Viewer
{
    /// <summary>
    /// Logique d'interaction pour Randomizer.xaml
    /// </summary>
    public partial class Randomizer : UserControl
    {
        RandomLogPlayer _randomPlayer;
        Application _app;
        bool _play = false;
        Task _task;

        int _depth;

        public Randomizer()
        {
            InitializeComponent();
            _randomPlayer = new RandomLogPlayer();
            _app = System.Windows.Application.Current;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!_play && _task==null)
            {
                _play = true;
                int sleepTime = 1000;
                ButtonPlay.Content = "Stop";
                EventManager.Instance.OnRegisterClient(_randomPlayer.GetRandomLog().Logger);
                _task = Task.Factory.StartNew((Action)delegate
                {
                    while (_play)
                    {
                        _app.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                        (Action)delegate
                        {
                            _randomPlayer.One(Convert.ToInt32(slider.Value));
                            sleepTime = Convert.ToInt32(slider2.Value);
                        });
                        Thread.Sleep(sleepTime);
                    }
                    _task = null;
                });
            } else if(_play) {
                _play = false;
                ButtonPlay.Content = "Play";
            }
        }
    }
}
