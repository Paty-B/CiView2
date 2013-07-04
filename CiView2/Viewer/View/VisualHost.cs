using CK.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Viewer.Model;

namespace Viewer.View
{
    public class VisualHost : FrameworkElement
    {
        private VisualCollection _children;
        private Point position = new Point(0,0);
        private ILineItemHost _host;
        private EnvironmentCreator ec;

        private int _maxPrintableLog = 60;


        public VisualHost()
        {

            ActivityLogger logger = new ActivityLogger();
            _host = LineItem.CreateLineItemHost();
            ec = new EnvironmentCreator(_host);
            
            _children = new VisualCollection(this);
            
            _host.ItemChanged += CheckEvents;
            logger.Output.RegisterClient(ec);


            #region generation log

            var tag1 = ActivityLogger.RegisteredTags.FindOrCreate("Product");
            var tag2 = ActivityLogger.RegisteredTags.FindOrCreate("Sql");
            var tag3 = ActivityLogger.RegisteredTags.FindOrCreate("Combined Tag|Sql|Engine V2|Product");



            using (logger.OpenGroup(LogLevel.None, () => "EndMainGroup", "MainGroup"))
            {
               using (logger.OpenGroup(LogLevel.Trace, () => "EndMainGroup", "MainGroup"))
                {
                    logger.Trace(tag1, "First2");
                    using (logger.AutoTags(tag1))
                    {
                        logger.Trace("Second2");
                        logger.Trace(tag3, "Third2");
                        using (logger.AutoTags(tag2))
                        {
                            logger.Info("First2");
                        }
                    }
                    using (logger.OpenGroup(LogLevel.Info, () => "Conclusion of Info Group (no newline).", "InfoGroup"))
                    {
                        logger.Info("Second3");
                        logger.Trace("Fourth3");

                        string warnConclusion = "Conclusion of Warn Group" + Environment.NewLine + "with more than one line int it.";
                        using (logger.OpenGroup(LogLevel.Warn, () => warnConclusion, "WarnGroup {0} - Now = {1}", 4, DateTime.UtcNow))
                        {
                            logger.Info("Warn!4");
                            logger.CloseGroup("User conclusion with multiple lines."
                                + Environment.NewLine + "It will be displayed on "
                                + Environment.NewLine + "multiple lines.");
                        }
                        logger.CloseGroup("Conclusions on one line are displayed separated by dash.");

                        logger.Info("T1");
                        logger.Trace("T2");
                        using (logger.OpenGroup(LogLevel.Info, () => "Conclusion of Info Group (no newline).", "InfoGroup"))
                        {
                            logger.Info("Second");
                            logger.Trace("Fourth");

                            using (logger.OpenGroup(LogLevel.Warn, () => warnConclusion, "WarnGroup {0} - Now = {1}", 4, DateTime.UtcNow))
                            {
                                logger.Error("error");
                                logger.Fatal("fatal content");

                            }
                        }
                    }
                }
            }
            
            #endregion

            EventManager.Instance.RegisterClient += RegisterClient;

            this.MouseLeftButtonUp += new MouseButtonEventHandler(VisualHost_MouseLeftButtonUp);
            this.MouseWheel += new MouseWheelEventHandler(Visual_Move);
            //EventManager.Instance.CheckBoxFilterTagClick += UpdateFromTagFilter;
            EventManager.Instance.CheckBoxFilterLogLevelClick += UpdateFromLogLevelFilter;

        }

        private void CheckEvents(object sender, LineItemChangedEventArgs e)
        {
            VisualLineItem vl;
            Point pt = new Point(0, 0);
            int index;

            switch(e.Status)
            {
                case LineItemChangedStatus.Collapsed:
                    //update visual
                    if (e.LineItem.GetType() == typeof(LogLineItem))
                    {
                        LogLineItem logLineItem = (LogLineItem)e.LineItem;
                        index = _children.IndexOf(logLineItem.vl);
                        _children.RemoveAt(index);
                        vl = e.LineItem.CreateVisualLine();
                        if (index != 0)
                        {
                            VisualLineItem lastLine = (VisualLineItem)_children[index - 1];
                            vl.Offset = new Vector(vl.Offset.X, lastLine.Offset.Y + 15);
                        }
                        _children.Insert(index, vl);

                        
                    }     
                    break;
                case LineItemChangedStatus.Deleted:
                    break;
                case LineItemChangedStatus.Expanded:
                    if (e.LineItem.GetType() == typeof(LogLineItem))
                    {
                        LogLineItem logLineItem = (LogLineItem)e.LineItem;
                        index = _children.IndexOf(logLineItem.vl);
                        _children.RemoveAt(index);
                        vl = e.LineItem.CreateVisualLine();
                        if (index != 0)
                        {
                            VisualLineItem lastLine = (VisualLineItem)_children[index - 1];
                            vl.Offset = new Vector(vl.Offset.X, lastLine.Offset.Y + 15);
                        }
                        _children.Insert(index, vl);
                    }   
                    break;
                case LineItemChangedStatus.Hidden:
                    if (e.LineItem.GetType() == typeof(LogLineItem))
                    {
                        LogLineItem logLineItem = (LogLineItem)e.LineItem;
                        index = _children.IndexOf(logLineItem.vl);
                        _children.RemoveAt(index);
                        vl = e.LineItem.CreateVisualLine();
                        if (index != 0)
                        {
                            VisualLineItem lastLine = (VisualLineItem)_children[index - 1];
                            vl.Offset = new Vector(vl.Offset.X, lastLine.Offset.Y + 15);
                        }

                        _children.Insert(index, vl);
                    }   
                    break;
                case LineItemChangedStatus.Inserted:
                    if (_children.Count == 0)
                    {
                        vl = e.LineItem.CreateVisualLine();
                        _children.Add(vl);
                        break;
                    }
                    if (_host.Root.TotalLineHeight < _maxPrintableLog 
                        || IsOnScreen((VisualLineItem)_children[_children.Count -1]))
                    {
                        VisualLineItem lastLine = (VisualLineItem)_children[_children.Count - 1];
                        vl = e.LineItem.CreateVisualLine();
                        vl.Offset = new Vector(vl.Offset.X, lastLine.Offset.Y + 15);
                        _children.Add(vl);
                    }
                    break;
                case LineItemChangedStatus.Update:
                    if (e.LineItem.GetType() == typeof(LogLineItem))
                    {
                        
                        LogLineItem logLineItem = (LogLineItem)e.LineItem;
                        index = _children.IndexOf(logLineItem.vl);
                        _children.RemoveAt(index);
                        vl = e.LineItem.CreateVisualLine();
                        _children.Insert(index, vl);
                    }  
                    break;
                case LineItemChangedStatus.Filtered:
                    if (e.LineItem.GetType() == typeof(LogLineItem))
                    {
                        LogLineItem logLineItem = (LogLineItem)e.LineItem;
                        index = _children.IndexOf(logLineItem.vl);
                        _children.RemoveAt(index);
                        vl = logLineItem.CreateFilteredVisualLine();
                        if (index != 0)
                        {
                            VisualLineItem lastLine = (VisualLineItem)_children[index - 1];
                            vl.Offset = new Vector(vl.Offset.X, lastLine.Offset.Y + 15);
                        }
                        _children.Insert(index, vl);
                    }   
                    break;
                case LineItemChangedStatus.Unfiltered:
                   if (e.LineItem.GetType() == typeof(LogLineItem))
                    {
                        LogLineItem logLineItem = (LogLineItem)e.LineItem;
                        index = _children.IndexOf(logLineItem.vl);
                        _children.RemoveAt(index);
                        vl = e.LineItem.CreateVisualLine();
                        if (index != 0)
                        {
                            VisualLineItem lastLine = (VisualLineItem)_children[index - 1];
                            vl.Offset = new Vector(vl.Offset.X, lastLine.Offset.Y + 15);
                        }

                        _children.Insert(index, vl);
                    }   
                    break;
            }
        }


        private void VisualHost_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //capture la position de la sourie dans mon framwork element
            System.Windows.Point pt = e.GetPosition((UIElement)sender);
            VisualTreeHelper.HitTest(this, null, new HitTestResultCallback(myCallback), new PointHitTestParameters(pt));
        }


        public HitTestResultBehavior myCallback(HitTestResult result)
        {
            if (result.VisualHit.GetType() == typeof(VisualGroupLineItem))
            {
                VisualGroupLineItem vlli = (VisualGroupLineItem)result.VisualHit;
                LogLineItem logLineItem = (LogLineItem)vlli.Model;
                int vlliIndex = _children.IndexOf(vlli);

                if (logLineItem.Status == Status.Expanded)
                {
                    logLineItem.Collapse();
                }
                else if (logLineItem.Status == Status.Collapsed)
                {
                    logLineItem.UnCollapse();
                }
                else
                {
                }
                
            }
            return HitTestResultBehavior.Stop;
        }




        #region UpdateTreeWithTagFilter

        public void UpdateFromTagFilter(string tag,bool isChecked)
        {
            
                LogLineItem FirstChild = (LogLineItem)_host.Root.FirstChild;

                var child = FirstChild;
                while (child != null)
                {
                    HaveFindTag(child,tag,isChecked);
                    var next = child.FirstChild;
                    while (next != null)
                    {
                        HaveFindTag((LogLineItem)next, tag, isChecked);
                        if (next.FirstChild == null)
                        {
                            next = next.Next;
                        }
                        else
                        {
                            next = next.FirstChild;
                        }
                    }
                    child = (LogLineItem)child.Next;
                }     
        }

        private bool HaveFindTag(LogLineItem LogLineItem, string tag,bool isChecked)
        {

            if (LogLineItem.Tag.IsEmpty)
                return false;
               foreach (CKTrait trait in LogLineItem.Tag.AtomicTraits)
                {
                    if (trait.ToString() == tag)
                    {
                        if (isChecked)
                        {
                            LogLineItem.Parent.InsertChild(new FilteredLineItem());
                        }
                        else
                        {
                            ILineItemParentImpl parent=LogLineItem.Parent;
                            ILineItem next=parent.FirstChild;
                            while(next!=null)
                            {
                                if (next.GetType() == typeof(FilteredLineItem))
                                {
                                     parent.RemoveChild(next);            
                                }
                            }
                           
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
      /*
            LogLineItem FirstChild = (LogLineItem)_host.Root.FirstChild;
            var child = FirstChild;
   
            while (child != null)
            {
                HaveFindLogLevel(child, loglevel, isChecked);
                var next = child.FirstChild;
                while (next != null)
                {
                    HaveFindLogLevel((LogLineItem)next, loglevel, isChecked);
                    if (next.FirstChild == null)
                    {
                        if (next.Next == null)                        
                            next = (LogLineItem)next.Parent.Parent.Next;
                        else
                            next = next.Next;           
                    }
                    else
                    {
                        next = next.FirstChild;
                    }
                }
                child = (LogLineItem)child.Next;
            }*/
            LogLineItem FirstChild = (LogLineItem)_host.Root.FirstChild;
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
                        if (parent == _host.Root.LastChild)
                            break;
                        child = parent;
                        parent = (LogLineItem)child.Parent;
                    }
                    child = (LogLineItem)parent.Next;
                    if(child != null && child.Parent!=_host.Root)
                        parent = (LogLineItem)child.Parent;
                    continue;
                    
                }
                child = (LogLineItem)child.Next;

            }
        }

     
        private bool HaveFindLogLevel(LogLineItem LogLineItem, string loglevel,bool isChecked)
        {
            if (LogLineItem.LogLevel.ToString() == loglevel)
            {
                if (isChecked)
                {

                    LogLineItem.unHidden();           
                }
                else  
                {
                    LogLineItem.Hidden();
                }
                return true;
            }
            else { return false; }
        }


        #endregion
        
                
        protected override int VisualChildrenCount
        {
            get { return _children.Count; }
        }
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }

        protected void updateVisual(VisualLineItem vli)
        {
            int index = _children.IndexOf(vli);
            
        }

        public void RegisterClient(ActivityLogger logger)
        {
            logger.Output.UnregisterClient(ec);
            logger.Output.RegisterClient(ec);
        }

        #region scrolling function

        private void Visual_Move(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                VisualLineItem lastLine = (VisualLineItem)_children[_children.Count - 1];
                VisualLineItem newLine = SelectNextVisualLine(lastLine, true);
                
                if (newLine != null)
                {
                    if (!IsOnScreen((VisualLineItem)_children[0]))
                    {
                        if (_children[0] != _host.Root.FirstChild.CreateVisualLine())
                            _children.RemoveAt(0);
                        newLine.Offset = new Vector(newLine.Offset.X, lastLine.Offset.Y + 15);
                        _children.Add(newLine);
                    }
                }
                if (((VisualLineItem)_children[_children.Count - 1]).Offset.Y > 0)
                    foreach (VisualLineItem vl in _children)
                        vl.Offset = new Vector(vl.Offset.X, vl.Offset.Y - 10);
            }
            if (e.Delta > 0)
            {
                VisualLineItem firstLine = (VisualLineItem)_children[0];
                VisualLineItem newLine = SelectNextVisualLine(firstLine, false);
                
                if (newLine != null)
                {
                    if(!IsOnScreen((VisualLineItem)_children[_children.Count - 1]))
                    {
                        if (_children[_children.Count - 1] != _host.Root.LastChild.CreateVisualLine())
                            _children.RemoveAt(_children.Count - 1);
                        newLine.Offset = new Vector(newLine.Offset.X, firstLine.Offset.Y - 15);
                        _children.Insert(0, newLine);
                    }
                }
                if (((VisualLineItem)_children[0]).Offset.Y <= 0)
                foreach (VisualLineItem vl in _children)
                    vl.Offset = new Vector(vl.Offset.X, vl.Offset.Y + 10);
            }
        }

        private bool IsOnScreen(VisualLineItem visualLine)
        {
            if (visualLine.Offset.Y >= 0 && visualLine.Offset.Y <= this.ActualHeight)
                return true;
            return false;
        }

        private VisualLineItem SelectNextVisualLine(VisualLineItem visualLine , bool downward)
        {
            if (visualLine.Model.Host.Root == visualLine.Model)
                return null;
            ILineItem lineItem = visualLine.Model;
            if (downward == true)
            {
                if (lineItem.FirstChild != null)
                    return lineItem.FirstChild.CreateVisualLine();
                if (lineItem.Next != null)
                    return lineItem.Next.CreateVisualLine();
                if (lineItem == lineItem.Parent.LastChild)
                    while (lineItem.Parent != null)
                    {
                        if (lineItem.Parent.Next != null)
                            return lineItem.Parent.Next.CreateVisualLine();
                        lineItem = lineItem.Parent;                      
                    }                   
            }

            if (downward == false)
            {
                if (lineItem.Prev != null)
                    return lineItem.Prev.CreateVisualLine();
                if (lineItem.Parent != null && lineItem.Parent != lineItem.Host.Root)
                    return lineItem.Parent.CreateVisualLine();
            }
            return null;
        }

        #endregion
    }
}