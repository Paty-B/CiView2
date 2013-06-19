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
        private int fontSize = 16;
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
                /*using (logger.OpenGroup(LogLevel.Trace, () => "EndMainGroup", "MainGroup"))
                {
                    logger.Trace(tag1, "First");
                    using (logger.AutoTags(tag1))
                    {
                        logger.Trace("Second");
                        logger.Trace(tag3, "Third");
                        using (logger.AutoTags(tag2))
                        {
                            logger.Info("First");
                        }
                    }
                    using (logger.OpenGroup(LogLevel.Info, () => "Conclusion of Info Group (no newline).", "InfoGroup"))
                    {
                        logger.Info("Second");
                        logger.Trace("Fourth");

                        string warnConclusion = "Conclusion of Warn Group" + Environment.NewLine + "with more than one line int it.";
                        using (logger.OpenGroup(LogLevel.Warn, () => warnConclusion, "WarnGroup {0} - Now = {1}", 4, DateTime.UtcNow))
                        {
                            logger.Info("Warn!");
                            logger.CloseGroup("User conclusion with multiple lines."
                                + Environment.NewLine + "It will be displayed on "
                                + Environment.NewLine + "multiple lines.");
                        }
                        logger.CloseGroup("Conclusions on one line are displayed separated by dash.");

                        logger.Info("T1");
                        logger.Trace("T2");
                    }
                }
                */

                EventManager.Instance.RegisterClient += RegisterClient;
            }

            #endregion

            this.MouseLeftButtonUp += new MouseButtonEventHandler(VisualHost_MouseLeftButtonUp);

            EventManager.Instance.CheckBoxFilterTagClick += UpdateFromTagFilter;
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

        public HitTestResultBehavior myCallback(HitTestResult result)
        {
            if (result.VisualHit.GetType() == typeof(VisualGroupLineItem))
            {
                VisualGroupLineItem vlli = (VisualGroupLineItem)result.VisualHit;
                LogLineItem logLineItem = (LogLineItem)vlli.Model;
                int vlliIndex = _children.IndexOf(vlli);

                logLineItem.toogleCollapse();
                
            }
            return HitTestResultBehavior.Stop;
        }




        #region UpdateTreeWithTagFilter

        public void UpdateFromTagFilter(string tag,bool isChecked)
        {
            
                LogLineItem FirstChild = (LogLineItem)_host.Root.FirstChild;
                UpdateFromTagFilterWhitoutRoot(FirstChild, tag,isChecked);
           
        }

        private void UpdateFromTagFilterWhitoutRoot(LogLineItem FirstChild, string tag,bool isChecked)
        {
           
            
            if (FirstChild != null)
            {
               HaveFindTag( FirstChild,tag,isChecked);
            }
            
            LogLineItem next=(LogLineItem)FirstChild.Next;
            while( next != null )
            {
                HaveFindTag( next,tag,isChecked);
                if(next.FirstChild!=null)
                {
                    UpdateFromTagFilterWhitoutRoot(FirstChild, tag, isChecked);
                }
                next=(LogLineItem)next.Next;
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
                UpdateFromLogLevelFilterWhitoutRoot(FirstChild, loglevel,isChecked);

        }

        private void UpdateFromLogLevelFilterWhitoutRoot(LogLineItem FirstChild, string loglevel,bool isChecked)
        {

            if (FirstChild != null)
            {
                HaveFindLogLevel(FirstChild, loglevel,isChecked);
            }

            LogLineItem next = (LogLineItem)FirstChild.Next;
            while (next != null)
            {
                HaveFindLogLevel(next, loglevel,isChecked);
                if (next.FirstChild != null)
                {
                    UpdateFromLogLevelFilterWhitoutRoot(FirstChild, loglevel,isChecked);
                }
                next = (LogLineItem)next.Next;
            }

        }
        private bool HaveFindLogLevel(LogLineItem LogLineItem, string loglevel,bool isChecked)
        {
            if (LogLineItem.LogLevel.ToString() == loglevel)
            {
                if (isChecked)
                {
                    LogLineItem.Status = Status.Expanded;
                    LogLineItem.Host.OnExpended(LogLineItem);
                }
                else
                {
                    LogLineItem.Status = Status.Collapsed;
                    LogLineItem.Host.OnCollapsed(LogLineItem);
                }
                return true;
            }
            else { return false; }
        }


        #endregion
        
        
        public void GoToLineItem(ILineItem lineItem)
        {
            position.X = lineItem.Depth;
            position.Y = lineItem.AbsoluteY;
            //mutiplicateur de position = position absolute de LineItem
        }
        
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