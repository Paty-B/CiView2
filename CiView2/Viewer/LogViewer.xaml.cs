using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Viewer.Model;
using Viewer.Model.Events;
using Viewer.View;

namespace Viewer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class LogViewer : Window
    {
        public ILineItemHost host;

  

        public LogViewer()
        {
            this.MouseWheel += new MouseWheelEventHandler(ScrollBarWheel);
      
        }

        private void ScrollBarSizeChanged(object sender, SizeScrollBarChangedEventArgs e)
        {
            scrollBar.ViewportSize = vHost.ActualHeight / 15;
            if (scrollBar.ViewportSize > e.Size)
                scrollBar.Maximum = 1;
            else
                scrollBar.Maximum = e.Size - scrollBar.ViewportSize;
        }

        public void ScrollBarWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
                scrollBar.Value++;
            else
                scrollBar.Value--;
        }

        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double difference = Math.Abs(e.NewValue - e.OldValue);
            if (e.OldValue < e.NewValue)
                this.vHost.scroll(true, difference*15);
            if(e.OldValue > e.NewValue)
                this.vHost.scroll(false, difference*15);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ILineItem root = vHost.GetLineItemHost().Root;
            LogLineItem currentLine = (LogLineItem)root.FirstChild;

            while(currentLine != null)
            {
                if (currentLine.Content.Contains(TextBox.Text))
                {
                    currentLine.unHidden();
                }
                else
                {                
                    currentLine.Hidden();
                }
                currentLine = (LogLineItem)SelectNextLine(currentLine, true);
            }
            
        }

        private ILineItem SelectNextLine(LogLineItem line, bool downward)
        {

            ILineItem lineItem = line;
            if (downward == true)
            {
                if (lineItem.FirstChild != null)
                    return lineItem.FirstChild;
                if (lineItem.Next != null)
                    return lineItem.Next;
                if (lineItem == lineItem.Parent.LastChild)
                    while (lineItem.Parent != null)
                    {
                        if (lineItem.Parent.Next != null)
                            return lineItem.Parent.Next;
                        lineItem = lineItem.Parent;
                    }
            }

            if (downward == false)
            {
                if (lineItem.Prev != null)
                {
                    if (lineItem.Prev.LastChild != null)
                    {
                        lineItem = lineItem.Prev.LastChild;
                        while (lineItem.LastChild != null)
                            lineItem = lineItem.LastChild;
                        return lineItem;
                    }
                    return lineItem.Prev;
                }
                if (lineItem.Parent != null && lineItem.Parent != lineItem.Host.Root)
                    return lineItem.Parent;
            }
            return null;
        }

    }
}
