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
using CK.Core;
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

            EventManager.Instance.CheckBoxFilterTagClick += UpdateFromTagFilter;
            EventManager.Instance.CheckBoxFilterLogLevelClick += UpdateFromLogLevelFilter;
      
        }

        public bool blocView()
        {
            return true;
        }

        private void ScrollBarSizeChanged(object sender, SizeScrollBarChangedEventArgs e)
        {
            PrintTextBox.Text = e.Size.ToString() + " lines printed.";
            scrollBar.ViewportSize = vHost.ActualHeight / 15;
            if (scrollBar.ViewportSize > e.Size)
            {
                scrollBar.Maximum = 0;
                vHost.DefaultPosition();
            }
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
                if (currentLine.Content.Contains(SearchTextBox.Text))
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

        #region UpdateTreeWithTagFilter

        public void UpdateFromTagFilter(string tag, bool isChecked)
        {
            LogLineItem FirstChild = (LogLineItem)vHost.GetLineItemHost().Root.FirstChild;
            var child = FirstChild;
            var parent = child;
            while (child != null)
            {
                HaveFindTag((LogLineItem)child, tag, isChecked);
                if (child.FirstChild != null)
                {
                    parent = child;
                    child = (LogLineItem)parent.FirstChild;
                    continue;
                }
                if (child == parent.LastChild)
                {
                    while (parent.Next == null)
                    {
                        if (parent == vHost.GetLineItemHost().Root.LastChild)
                            break;
                        child = parent;
                        parent = (LogLineItem)child.Parent;
                    }
                    child = (LogLineItem)parent.Next;
                    if (child != null && child.Parent != vHost.GetLineItemHost().Root)
                        parent = (LogLineItem)child.Parent;
                    continue;

                }
                child = (LogLineItem)child.Next;
            }
        }

        private bool HaveFindTag(LogLineItem LogLineItem, string tag, bool isChecked)
        {

            if (LogLineItem.Tag.IsEmpty)
                return false;
            foreach (CKTrait trait in LogLineItem.Tag.AtomicTraits)
            {
                if (trait.ToString() == tag)
                {
                    if (isChecked)
                    {
                        if (this._userControl._logLevelFilters._listBoxOfCheckBoxCounter.IsLogLevelOrTagChecked(LogLineItem.LogLevel.ToString())) 
                            LogLineItem.unHidden();
                    }
                    else
                    {
                        LogLineItem.Filtered();
                       
                    }
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region UpdateTreeWithLogLevelFilter

        public void UpdateFromLogLevelFilter(string loglevel, bool isChecked)
        {
            LogLineItem FirstChild = (LogLineItem)vHost.GetLineItemHost().Root.FirstChild;
            var child = FirstChild;
            var parent = child;
            while (child != null)
            {
                HaveFindLogLevel(child, loglevel, isChecked);
                if (child.FirstChild != null)
                {
                    parent = child;
                    child = (LogLineItem)parent.FirstChild;
                    continue;
                }
                if (child == parent.LastChild)
                {
                    while (parent.Next == null)
                    {
                        if (parent == vHost.GetLineItemHost().Root.LastChild)
                            break;
                        child = parent;
                        parent = (LogLineItem)child.Parent;
                    }
                    child = (LogLineItem)parent.Next;
                    if (child != null && child.Parent != vHost.GetLineItemHost().Root)
                        parent = (LogLineItem)child.Parent;
                    continue;

                }
                child = (LogLineItem)child.Next;

            }
        }


        private bool HaveFindLogLevel(LogLineItem LogLineItem, string loglevel, bool isChecked)
        {
            if (LogLineItem.LogLevel.ToString() == loglevel)
            {
                if (isChecked)
                {
                    if (this._userControl._tagFilters._listBoxOfCheckBoxCounter.IsLogLevelOrTagChecked(LogLineItem.Tag.ToString())) 
                        LogLineItem.unHidden();
                }
                else
                {
                    LogLineItem.Filtered();
                }
                return true;
            }
            else { return false; }
        }


        #endregion

    }

     
        
}
