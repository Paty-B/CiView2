﻿using System;
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

        public void ScrollBarWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
                scrollBar.Value++;
            else
                scrollBar.Value--;
        }

        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            if (vHost.GetLineItemHost().Root.TotalLineHeight != 0)
            {
                
                scrollBar.ViewportSize = vHost.ActualHeight/15;
                scrollBar.Minimum = 0;
                scrollBar.Maximum = vHost.GetLineItemHost().Root.TotalLineHeight - scrollBar.ViewportSize;

                
                double difference = Math.Abs(e.NewValue - e.OldValue);
                if (e.OldValue < e.NewValue)
                    this.vHost.scroll(true, difference*15);
                if(e.OldValue > e.NewValue)
                    this.vHost.scroll(false, difference*15);
                
            }
        }

    }
}
