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
            _children = new VisualCollection(this);
            CreateFakeLog();
            


            this.MouseLeftButtonUp += new MouseButtonEventHandler(VisualHost_MouseLeftButtonUp);

        }

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
        private void VisualHost_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //capture la position de la sourie dans mon framwork element
            System.Windows.Point pt = e.GetPosition(this);
        }


        private DrawingVisual DrawLogLineItem(LogLineItem logLineItem)
        {
            DrawingVisual drawingLogLineItem = new DrawingVisual();
            DrawingContext drawingContext = drawingLogLineItem.RenderOpen();
            Point drawingPosition = new Point(logLineItem.Depth-position.X, Math.Round((double)((logLineItem.AbsoluteY-position.Y) * fontSize)));


            VisualDesigner.CreateExpander(drawingContext, drawingPosition, logLineItem.Status);
            drawingPosition.X += fontSize;
            VisualDesigner.CreateSymbol(drawingContext, drawingPosition, logLineItem.LogLevel);
            drawingPosition.X += fontSize;
            VisualDesigner.CreateContent(drawingContext, drawingPosition, logLineItem.Content);
            drawingPosition.X += Math.Round((double)fontSize * logLineItem.Content.Length);
            VisualDesigner.CreateTag(drawingContext, drawingPosition, logLineItem.Tag);
            drawingPosition.X += Math.Round((double)fontSize * logLineItem.Tag.ToString().Length);
            VisualDesigner.CreateNextLogIndicator(drawingContext, drawingPosition, logLineItem.NbWarning, logLineItem.NbError, logLineItem.NbFatal);


            drawingContext.Close();
            return drawingLogLineItem;
        }

        public DrawingVisual DrawFiltredLineItem(FilteredLineItem filtredLineItem)
        {
            DrawingVisual drawingFiltredLineItem = new DrawingVisual();
            DrawingContext drawingContext = drawingFiltredLineItem.RenderOpen();
            Point drawingPosition = new Point(filtredLineItem.Depth - position.X, Math.Round((double)((filtredLineItem.AbsoluteY - position.Y) * fontSize)));

            VisualDesigner.CreateFiltredLogRepresentation(drawingContext, drawingPosition);

            drawingContext.Close();
            return drawingFiltredLineItem;
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
            BitmapImage fatalImg = new BitmapImage(new Uri(@"C:\Users\Paty\Documents\Dev\CiView2\CiView2\Viewer\img\fatal15.png"));
            BitmapImage errorImg = new BitmapImage(new Uri (@"C:\Users\Paty\Documents\Dev\CiView2\CiView2\Viewer\img\error15.png"));
            BitmapImage warningImg = new BitmapImage(new Uri(@"C:\Users\Paty\Documents\Dev\CiView2\CiView2\Viewer\img\warning15.png"));
            BitmapImage logLevelImg;

            switch (loglevel)
            {
                case LogLevel.Warn:
                    logLevelImg = warningImg;
                    break;
                case LogLevel.Error:
                    logLevelImg = errorImg;
                    break;
                case LogLevel.Fatal:
                    logLevelImg = fatalImg;
                    break;
                default:
                    logLevelImg = new BitmapImage(new Uri(@"C:\Users\Paty\Documents\Dev\CiView2\CiView2\Viewer\img\no15.png"));
                    break;

            }

            drawingContext.DrawImage(logLevelImg, new Rect(pt.X, pt.Y + 2, errorImg.PixelWidth, errorImg.PixelHeight));

            drawingContext.Close();

            return drawingVisual;
        }

        public void GoToLineItem(LineItem lineItem)
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
