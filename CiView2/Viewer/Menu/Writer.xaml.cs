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

namespace Viewer
{
    /// <summary>
    /// Logique d'interaction pour Writer.xaml
    /// </summary>
    public partial class Writer : UserControl
    {
        public Writer()
        {
            InitializeComponent();
        }

        private void ButtonWriterEditor_Click(object sender, RoutedEventArgs e)
        {
            WriterEditor window = new WriterEditor();
            window.ShowDialog();
        }
    }
}
