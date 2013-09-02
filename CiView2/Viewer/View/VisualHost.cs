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
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using Viewer.Model.Events;

namespace Viewer.View
{
    public class VisualHost : FrameworkElement
    {
        private VisualCollection _children;
        private Point position = new Point(0,0);
        private ILineItemHost _host;
        private EnvironmentCreator ec;
        private int _nbVisualElement = 0;
        private int _maxPrintableLog = 60;

        public event EventHandler<SizeScrollBarChangedEventArgs> LinesChanged;

        public ListBoxOfCheckBoxCounter _logLevels = null;
        public ListBoxOfCheckBoxCounter _tags = null;

        public VisualHost()
        {
            
            ActivityLogger logger = new ActivityLogger();
            _host = LineItem.CreateLineItemHost();
            ec = new EnvironmentCreator(_host);
            
            _children = new VisualCollection(this);
            
            _host.ItemChanged += CheckEvents;
            logger.Output.RegisterClient(ec);

            #region generation log
            /*
            var tag1 = ActivityLogger.RegisteredTags.FindOrCreate("Product");
            var tag2 = ActivityLogger.RegisteredTags.FindOrCreate("Sql");
            var tag3 = ActivityLogger.RegisteredTags.FindOrCreate("Combined Tag|Sql|Engine V2|Product");



            using (logger.OpenGroup(LogLevel.Info, () => "EndMainGroup", "Begin dbSetup with:"))
            {
               logger.Info(@"RootPath: D:\Invenietis\Dev\Dev4\Cofely-BO\");
               logger.Info(@"FilePaths:");
               logger.Info(@"DllPaths: CFLY.Data.Setup\bin\Debug");
               logger.Info(@"Assembly: CFLY.Data.Setup, CK.DB.Basic, CK.Authentication.Local");
            }
            using (logger.OpenGroup(LogLevel.Info, () => "EndMainGroup", "Connection"))
            {
                logger.Info(@" Connected to .\SQLSERVER2012/Cofely");
            }
            using (logger.OpenGroup(LogLevel.Trace, () => "EndMainGroup", "First setup"))
            {
                using (logger.OpenGroup(LogLevel.Info, () => "", "Collecting objects."))
                {
                    using (logger.OpenGroup(LogLevel.Info, () => "", "Discovering assemblies & types from configuration."))
                    {
                        logger.Trace(@"Discovering assembly 'CFLY.Data.Setup, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null'.");
                        logger.Trace(@"Discovering assembly 'CK.DB.Basic, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null'.");
                        logger.Trace(@"Discovering assembly 'CK.Authentication.Local, Version=2.3.0.0, Culture=neutral, PublicKeyToken=null'.");
                    }
                    using (logger.OpenGroup(LogLevel.Trace, () => "", "Registering 3 assemblies"))
                    {
                        logger.Trace(@"Registering assembly 'CFLY.Data.Setup, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null'.");
                        logger.Trace(@"Registering assembly 'CK.DB.Basic, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null'.");
                        logger.Trace(@"Registering assembly 'CK.Authentication.Local, Version=2.3.0.0, Culture=neutral, PublicKeyToken=null'.");
                    }
                }
                using (logger.OpenGroup(LogLevel.Info, () => "", "Collecting Ambient Contracts and Type structure."))
                {
                    using (logger.OpenGroup(LogLevel.Trace, () => "", "Ambient Contract discovering: 1 context(s)."))
                    {
                        logger.Trace(@"Ambient Contract for context '': 47 mappings for 43 concrete paths.");
                    }
                }
                using (logger.OpenGroup(LogLevel.Info, () => "", "Creating Structure Objects."))
                {
                    logger.Info(@"Working on Context [].");
                }
                using (logger.OpenGroup(LogLevel.Info, () => "", "Handling dependencies."))
                {
                    using (logger.OpenGroup(LogLevel.Info, () => "", "Working on Context []."))
                    {
                        using (logger.OpenGroup(LogLevel.Trace, () => "", "Preparing '[]CFLY.Data.Setup.Cultures.CultureHome'."))
                        {
                            logger.Trace(@"Preparing '[]CFLY.Data.Setup.Cultures.CulturesPackage'.");
                        }
                        using (logger.OpenGroup(LogLevel.Trace, () => "", "Preparing '[]CFLY.Data.Setup.DSP.Activities.ActivityHome'."))
                        {
                            logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Activities.ActivitiesPackage'.");
                        }
                        using (logger.OpenGroup(LogLevel.Trace, () => "", "Preparing '[]CFLY.Data.Setup.DSP.ExternalVersioning.ExternalVersioningHome'."))
                        {
                            logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.ExternalVersioning.ExternalVersioningPackage'.");
                        }
                        using (logger.OpenGroup(LogLevel.Trace, () => "", "Preparing '[]CFLY.Data.Setup.DSP.Notifications.NotificationHistoryHome'."))
                        {
                            logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Notifications.NotificationsPackage'.");
                        }
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Notifications.DeviceTokenHome'.");
                        using (logger.OpenGroup(LogLevel.Trace, () => "", "Preparing '[]CFLY.Data.Setup.DSP.Refs.ContentBindingHome'."))
                        {
                            logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Notifications.NotificationsPackage'.");
                        }
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Refs.VersionedRefSearchableContentHome'.");
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Refs.DetailsArticleHome'.");
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Refs.SolutionBindingHome'.");
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Refs.KeyDataHome'.");
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Refs.DocumentLocationHome'.");
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Refs.VersionedRefTargetHome'.");
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Refs.VersionedRefHome'.");
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Refs.RefHome'.");
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Refs.DocumentHome'.");
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Refs.FileHome'.");
                        logger.Trace(@"Preparing '[]CFLY.Data.Setup.DSP.Refs.TargetHome'.");
                        using (logger.OpenGroup(LogLevel.Trace, () => "", "Preparing '[]CFLY.Data.Setup.DSP.Themes.ContentHome'."))
                        {
                            logger.Trace(@"Preparing '[]CFLY.Data.Setup.ThemesPackage'.");
                        }

                    }

                }



            }

            //*/



              /* using (logger.OpenGroup(LogLevel.Trace, () => "EndMainGroup", "MainGroup"))
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
            }*/
            
            #endregion

            EventManager.Instance.RegisterClient += RegisterClient;

            this.MouseLeftButtonUp += new MouseButtonEventHandler(VisualHost_MouseLeftButtonUp);
           
        }

        public void InitializeBoxes(ListBoxOfCheckBoxCounter tags, ListBoxOfCheckBoxCounter logLevels)
        {
            _tags = tags;
            _logLevels = logLevels;
        }

        public ILineItemHost GetLineItemHost()
        {
            return _host;
        }

        public int GetNbVisualElement()
        {
            
            return _nbVisualElement;
        }

        private void CheckEvents(object sender, LineItemChangedEventArgs e)
        {
            VisualLineItem vl;
            Point pt = new Point(0, 0);
            int index = 0;
            var _LinesChanged = LinesChanged;

            switch(e.Status)
            {
                case LineItemChangedStatus.Inserted:
                    if (e.LineItem.GetType() == typeof(LogLineItem))
                    {
                        vl = e.LineItem.CreateVisualLine();
                        if (_children.Count != 0)
                        {
                            VisualLineItem lastLine = (VisualLineItem)_children[_children.Count - 1];
                            vl.Offset = new Vector(vl.Offset.X, lastLine.Offset.Y + 15);
                        }
                        LogLineItem current = (LogLineItem)e.LineItem;
                        LogLineItem Parent = null;
                        _nbVisualElement++;
                        _children.Add(vl);
                        if ((Parent = current.GetParentCollapsed()) != null)
                        {
                            ((LogLineItem)vl.Model).Hidden();
                            break;
                        }
                        if(_logLevels != null && !_logLevels.IsCaseChecked(current.LogLevel.ToString()))
                        {
                            ((LogLineItem)vl.Model).Hidden();
                            break;
                        }
                        if(_tags != null && _tags.caseExist(current.Tag.ToString()) == true)
                        {
                            if(!_tags.IsCaseChecked(current.Tag.ToString()))
                            {
                                ((LogLineItem)vl.Model).Hidden();
                            }   
                        }
                  
                    }

                    break;

                case LineItemChangedStatus.Visible:
                    if (e.LineItem.GetType() == typeof(LogLineItem))
                    {
                        
                        LogLineItem logLineItem = (LogLineItem)e.LineItem;
                        if (!_tags.IsCaseChecked(logLineItem.Tag.ToString())
                            || !_logLevels.IsCaseChecked(logLineItem.LogLevel.ToString()))
                        {
                            _nbVisualElement++;
                            logLineItem.Hidden();
                            break;
                        }
                        index = _children.IndexOf(logLineItem.vl);
                        vl = e.LineItem.CreateVisualLine();
                       
                        if (index == 0)
                        {
                            if (_children.Count == 1)
                                vl.Offset = new Vector(vl.Offset.X, vl.Offset.Y);
                            else
                            {
                                VisualLineItem previousLine = (VisualLineItem)_children[1];
                                vl.Offset = new Vector(vl.Offset.X, previousLine.Offset.Y - 15);
                            }
                            _children.RemoveAt(0);
                            if (((LogLineItem)e.LineItem).OldStatus == Status.Hidden)
                                _nbVisualElement++;
                            _children.Insert(index, vl);
                            DefaultPosition();
                            break;
                        }
                          
                        VisualLineItem lastLine = (VisualLineItem)_children[index - 1];
                        if (((LogLineItem)e.LineItem).OldStatus == Status.Hidden)
                            _nbVisualElement++;
                           
                        vl.Offset = new Vector(vl.Offset.X, lastLine.Offset.Y + 15);
                        _children.RemoveAt(index);
                           
                        _children.Insert(index, vl);
                        RefreshVisual(vl);
                        
                    }
                    break;

                case LineItemChangedStatus.Invisible:
                    if (e.LineItem.GetType() == typeof(LogLineItem))
                    {
                        LogLineItem logLineItem = (LogLineItem)e.LineItem;
                        index = _children.IndexOf(logLineItem.vl);
                        vl = e.LineItem.CreateVisualLine();
                      
                        if (index == 0)
                        {
                            vl.Offset = new Vector(vl.Offset.X, vl.Offset.Y);
                            _children.RemoveAt(0);
                            if (((LogLineItem)e.LineItem).OldStatus != Status.Hidden )
                                _nbVisualElement--;
                            _children.Insert(index, vl);
                            break;
                        }
                        VisualLineItem lastLine = (VisualLineItem)_children[index - 1];
                        vl.Offset = new Vector(vl.Offset.X, lastLine.Offset.Y);
                        _children.RemoveAt(index);
                        if (((LogLineItem)e.LineItem).OldStatus != Status.Hidden)
                            _nbVisualElement--;
                        _children.Insert(index, vl);
                        RefreshVisual(vl);
                        
                    }
                    break;

            }
            if(_LinesChanged != null)
                _LinesChanged(this, new SizeScrollBarChangedEventArgs(_nbVisualElement));           
        }

        public void DefaultPosition()
        {
            RefreshVisual((VisualLineItem)_children[0]);
        }

        private void RefreshVisual(VisualLineItem vl)
        {
            int index = _children.IndexOf(vl);
            VisualLineItem lastLine;
            VisualLineItem currentLine;
            LogLineItem temp;
            for (int i = index+1; i < _children.Count; i++)
            {
                lastLine = (VisualLineItem)_children[i-1];
                currentLine = (VisualLineItem)_children[i];
                _children.RemoveAt(i);
                temp = (LogLineItem)currentLine.Model;
                if(temp.Status == Status.Hidden)
                    currentLine.Offset = new Vector(currentLine.Offset.X, lastLine.Offset.Y);
                else if (currentLine.GetType() == typeof(VisualFilteredLineItem))
                {
                    if(lastLine.GetType() == typeof(VisualFilteredLineItem))
                        currentLine.Offset = new Vector(currentLine.Offset.X, lastLine.Offset.Y);
                    else
                        currentLine.Offset = new Vector(currentLine.Offset.X, lastLine.Offset.Y + 15);

                }

                else
                    currentLine.Offset = new Vector(currentLine.Offset.X, lastLine.Offset.Y + 15);
                _children.Insert(i, currentLine);

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
                    logLineItem.Expand();
                }
                else
                {
                }
                
            }
            return HitTestResultBehavior.Stop;
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

        #region scrolling function

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
                {
                    if (lineItem.Prev.LastChild != null)
                    {
                        lineItem = lineItem.Prev.LastChild;
                        while (lineItem.LastChild != null)
                            lineItem = lineItem.LastChild;
                        return lineItem.CreateVisualLine();
                    }                       
                    return lineItem.Prev.CreateVisualLine();
                }
                if (lineItem.Parent != null && lineItem.Parent != lineItem.Host.Root)
                    return lineItem.Parent.CreateVisualLine();
            }
            return null;
        }
        
        public void scroll(bool downWard, double speed = 15)
        {
            if (downWard == true)
            {
                VisualLineItem lastLine = (VisualLineItem)_children[_children.Count - 1];
                VisualLineItem newLine = SelectNextVisualLine(lastLine, true);
/*
                if (newLine != null)
                {
                    if (!IsOnScreen((VisualLineItem)_children[0]))
                    {
                        if (_children[0] != _host.Root.FirstChild.CreateVisualLine())
                            _children.RemoveAt(0);
                        newLine.Offset = new Vector(newLine.Offset.X, lastLine.Offset.Y + 15);
                        _children.Add(newLine);
                    }
                }*/
                if (((VisualLineItem)_children[_children.Count - 1]).Offset.Y > 0)
                    foreach (VisualLineItem vl in _children)
                        vl.Offset = new Vector(vl.Offset.X, vl.Offset.Y - speed);
            }
            if (downWard == false)
            {
                VisualLineItem firstLine = (VisualLineItem)_children[0];
                VisualLineItem newLine = SelectNextVisualLine(firstLine, false);
/*
                if (newLine != null)
                {
                    if (!IsOnScreen((VisualLineItem)_children[_children.Count - 1]))
                    {
                        if (_children[_children.Count - 1] != _host.Root.LastChild.CreateVisualLine())
                            _children.RemoveAt(_children.Count - 1);
                        newLine.Offset = new Vector(newLine.Offset.X, firstLine.Offset.Y - 15);
                        _children.Insert(0, newLine);
                    }
                }*/
                if (((VisualLineItem)_children[0]).Offset.Y <= 0)
                    foreach (VisualLineItem vl in _children)
                        vl.Offset = new Vector(vl.Offset.X, vl.Offset.Y + speed);
            }
        }

        #endregion
    }
}