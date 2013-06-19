using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using CK.Core;

namespace Randomizer
{
    /// <summary>
    /// Logique d'interaction pour UCWindow.xaml
    /// </summary>
    public partial class UCWindow : UserControl
    {
        RandomLogPlayer _randomPlayer;
        Timer timer;       
        bool play = false;
        Application app;

        int _depth;
        public int depth
        {
            get
            {
                return (int)GetValue(DepthProperty);
            }
            set
            {
                SetValue(DepthProperty, value);
                _depth = depth;
            }
        }
         
        public UCWindow()
        {
            InitializeComponent();
            _randomPlayer = new RandomLogPlayer();
            this.DataContext = this;  
            timer=new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(Boucle);
            timer.Enabled = true;
            app = System.Windows.Application.Current;
        }

        private void Boucle(object source, ElapsedEventArgs e)
        {           
            if (play == true)
                app.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                                   (Action)delegate
                {
                    _randomPlayer.One(_depth);
                });     
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
      
            if (play == false)
                play = true;
            else
                play = false;
        }

        public static readonly DependencyProperty DepthProperty =
            DependencyProperty.Register("Depth", typeof(int), typeof(UCWindow), new PropertyMetadata(null));     
    }
}
