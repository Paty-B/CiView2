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
            //this.MouseWheel += new MouseWheelEventHandler(VisualHost_MouseWheel);

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
                        _children.Insert(index, vl);
                    }     
                    break;
                case LineItemChangedStatus.Deleted :
                    break;
                case LineItemChangedStatus.Expanded:
                    if (e.LineItem.GetType() == typeof(LogLineItem))
                    {
                        LogLineItem logLineItem = (LogLineItem)e.LineItem;
                        index = _children.IndexOf(logLineItem.vl);
                        _children.RemoveAt(index);
                        vl = e.LineItem.CreateVisualLine();
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
                        _children.Insert(index, vl);
                    }   
                    break;
                case LineItemChangedStatus.Inserted:
                    vl = e.LineItem.CreateVisualLine();
                    _children.Add(vl);                    
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
            }
        }

        private void VisualHost_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //capture la position de la sourie dans mon framwork element
            System.Windows.Point pt = e.GetPosition((UIElement)sender);
            VisualTreeHelper.HitTest(this, null, new HitTestResultCallback(myCallback), new PointHitTestParameters(pt));
        }

        /*private void VisualHost_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int move = e.Delta;
            foreach (VisualLineItem vl in _children)
            {
                vl.Offset = new Vector(0, move);
            }
        }*/

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

     
        private bool HaveFindLogLevel(LogLineItem LogLineItem, string loglevel,bool isChecked)
        {
            if (LogLineItem.LogLevel.ToString() == loglevel)
            {
                if (isChecked)
                {
                    LogLineItem.Status = Status.Expanded;
                    LogLineItem.UnCollapse();
                }
                else
                {
                    LogLineItem.Status = Status.Collapsed;
                    LogLineItem.Collapse();
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
    }
}