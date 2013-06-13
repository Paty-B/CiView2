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


        public VisualHost()
        {

            ActivityLogger logger = new ActivityLogger();
            ILineItemHost host = LineItem.CreateLineItemHost();
            EnvironmentCreator ec = new EnvironmentCreator(host);
            
            _children = new VisualCollection(this);
            
            host.ItemChanged += CheckEvents;
            logger.Output.RegisterClient(ec);


            #region generation log

            var tag1 = ActivityLogger.RegisteredTags.FindOrCreate("Product");
            var tag2 = ActivityLogger.RegisteredTags.FindOrCreate("Sql");
            var tag3 = ActivityLogger.RegisteredTags.FindOrCreate("Combined Tag|Sql|Engine V2|Product");



            logger.Trace("Second");

            /*using (logger.OpenGroup(LogLevel.None, () => "EndMainGroup", "MainGroup"))
            {
                using (logger.OpenGroup(LogLevel.Trace, () => "EndMainGroup", "MainGroup"))
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
                    }
                }
            }*/

            #endregion
            

            this.MouseLeftButtonUp += new MouseButtonEventHandler(VisualHost_MouseLeftButtonUp);

        }

        private void CheckEvents(object sender, LineItemChangedEventArgs e)
        {
            VisualLineItem vl;
            Point pt = new Point(0, 0);

            switch(e.Status)
            {
                case LineItemChangedStatus.Collapsed:
                    break;
                case LineItemChangedStatus.Deleted :
                    break;
                case LineItemChangedStatus.Expanded:
                    break;
                case LineItemChangedStatus.Hidden:
                    break;
                case LineItemChangedStatus.Inserted:
                    vl = e.LineItem.CreateVisualLine();
                    _children.Add(CreateDrawingVisualText("main log", LogLevel.Info, pt));
                    _children.Add(vl);
                    break;
            }
        }


        #region fake

        private void CreateFakeLog()
        {
            Point pt = new Point(0, 0);
            _children.Add(CreateDrawingVisualText("main log", LogLevel.Info, pt));
            _children.Add(CreateDrawingVisualSymbol(LogLevel.Info, new Point(pt.X - fontSize, pt.Y)));
            pt = incrementOnce(pt);
            _children.Add(CreateDrawingVisualText("second log", LogLevel.Error, pt));
            _children.Add(CreateDrawingVisualSymbol(LogLevel.Error, new Point(pt.X - fontSize, pt.Y)));
            pt = downOne(pt);
            _children.Add(CreateDrawingVisualText("third log", LogLevel.Error, pt));
            _children.Add(CreateDrawingVisualSymbol(LogLevel.Error, new Point(pt.X - fontSize, pt.Y)));
            pt = incrementOnce(pt);
            _children.Add(CreateDrawingVisualText("groupe 1.1", LogLevel.Info, pt));
            _children.Add(CreateDrawingVisualSymbol(LogLevel.Info, new Point(pt.X - fontSize, pt.Y)));
            pt = incrementOnce(pt);
            _children.Add(CreateDrawingVisualText("log", LogLevel.Error, pt));
            _children.Add(CreateDrawingVisualSymbol(LogLevel.Error, new Point(pt.X - fontSize, pt.Y)));
            pt = downOne(pt);
            _children.Add(CreateDrawingVisualText("log", LogLevel.Fatal, pt));
            _children.Add(CreateDrawingVisualSymbol(LogLevel.Fatal, new Point(pt.X - fontSize, pt.Y)));
            pt = decrementOnce(pt);
            _children.Add(CreateDrawingVisualText("groupe 1.2", LogLevel.Warn, pt));
            _children.Add(CreateDrawingVisualSymbol(LogLevel.Warn, new Point(pt.X - fontSize, pt.Y)));
    
        }

        private Point incrementOnce(Point pt)
        {
            pt.X = pt.X + 20;
            pt = downOne(pt);
            return pt;
        }
        private Point decrementOnce(Point pt)
        {
            pt.X = pt.X - 20;
            pt = downOne(pt);
            return pt;
        }
        private Point downOne(Point pt)
        {
            pt.Y = pt.Y + fontSize;
            return pt;
        }
        private DrawingVisual CreateDrawingVisualText(String text, LogLevel loglevel, Point pt)
        {
            
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            SolidColorBrush logColor = System.Windows.Media.Brushes.Black;

            drawingContext.DrawText(
               new FormattedText(text,
                  CultureInfo.GetCultureInfo("en-us"),
                  FlowDirection.LeftToRight,
                  new Typeface("Consolas"),
                  fontSize, logColor),
                  pt);

            drawingContext.Close();
            return drawingVisual;
        }
        private DrawingVisual CreateDrawingVisualSymbol(LogLevel loglevel, Point pt)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            BitmapImage testImg = new BitmapImage();
            BitmapImage fatalImg = new BitmapImage(new Uri(@"C:\Users\Paty\Documents\Dev\CiView2\CiView2\Viewer\img\fatal15.png"));
            BitmapImage errorImg = new BitmapImage(new Uri(@"C:\Users\Paty\Documents\Dev\CiView2\CiView2\Viewer\img\error15.png"));
            BitmapImage warningImg = new BitmapImage(new Uri(@"C:\Users\Paty\Documents\Dev\CiView2\CiView2\Viewer\img\warning15.png"));
            BitmapImage logLevelImg;

            switch (loglevel)
            {
                case LogLevel.Warn:
                    //logLevelImg = warningImg;
                    logLevelImg = testImg;
                    break;
                case LogLevel.Error:
                    //logLevelImg = errorImg;
                    logLevelImg = testImg;
                    break;
                case LogLevel.Fatal:
                    //logLevelImg = fatalImg;
                    logLevelImg = testImg;
                    break;
                default:

                    logLevelImg = testImg;
                    //new BitmapImage(new Uri(@"C:\Users\Paty\Documents\Dev\CiView2\CiView2\Viewer\img\no15.png"));
                    break;

            }

            drawingContext.DrawImage(logLevelImg, new Rect(pt.X, pt.Y + 2, errorImg.PixelWidth, errorImg.PixelHeight));

            drawingContext.Close();

            return drawingVisual;
        }

        #endregion

        private void VisualHost_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //capture la position de la sourie dans mon framwork element
            System.Windows.Point pt = e.GetPosition(this);
        }


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
    }
}
