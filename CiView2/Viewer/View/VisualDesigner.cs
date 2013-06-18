using CK.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Viewer.Model;

namespace Viewer.View
{
    public static class VisualDesigner
    {
        static int frontSize = 14;

        public static void CreateExpander(DrawingContext dc, Point pt, Status status)
        {
            BitmapImage bi;
            Size size = new Size(frontSize, frontSize);

            switch (status)
            {
                case Status.Collapsed:
                    bi = ImageRequest.Instance.GetImage("expander.ico");
                    break;
                case Status.Expanded:
                    bi = ImageRequest.Instance.GetImage("expander.ico");
                    bi.Rotation = Rotation.Rotate270;
                    break;
                //case Status.Hidden:
                  //  break;
                default:
                    bi = ImageRequest.Instance.GetImage("noImage.ico");
                    break;
            }
            dc.DrawImage(bi, new Rect(pt, size));

        }
        public static void CreateSymbol(DrawingContext dc, Point pt, LogLevel logLevel)
        {
            BitmapImage bi;
            Size size = new Size(frontSize,frontSize);

            switch (logLevel)
            {
                case LogLevel.Trace :
                    bi = ImageRequest.Instance.GetImage("trace.ico");
                    break;
                case LogLevel.Info:
                    bi = ImageRequest.Instance.GetImage("info.ico");
                    break;
                case LogLevel.Warn:
                    bi = ImageRequest.Instance.GetImage("warning.ico");
                    break;
                case LogLevel.Error:
                    bi = ImageRequest.Instance.GetImage("error.ico");
                    break;
                case LogLevel.Fatal:
                    bi = ImageRequest.Instance.GetImage("fatal.ico");
                    break;
                case LogLevel.None:
                    bi = ImageRequest.Instance.GetImage("noImage.ico");
                    break;
                default:
                    bi = ImageRequest.Instance.GetImage("noImage.ico");
                    break;
            } 
            dc.DrawImage(bi, new Rect(pt,size));
        }
        public static void CreateContent(DrawingContext drawingContext, Point pt, String content, int lineHeight)
        {
            FormattedText ft = new FormattedText(
                                content,
                                CultureInfo.GetCultureInfo("en-us"), 
                                FlowDirection.LeftToRight, 
                                new Typeface(new FontFamily("Consolas"), 
                                FontStyles.Normal, 
                                FontWeights.Bold, 
                                FontStretches.Normal),
                                frontSize,
                                Brushes.Black);
            drawingContext.DrawText(ft, pt);
        }
        public static void CreateTag(DrawingContext dc, Point pt, CKTrait tag)
        {
            FormattedText ft = new FormattedText(tag.ToString(),
                                CultureInfo.GetCultureInfo("en-us"),
                                FlowDirection.LeftToRight,
                                new Typeface(new FontFamily("Consolas"),
                                FontStyles.Normal,
                                FontWeights.Bold,
                                FontStretches.Normal),
                                frontSize,
                                Brushes.Black);
            dc.DrawText(ft, pt);

        }
        public static void CreateNextImportantLogsIndicator(DrawingContext drawingContext, ILineItem model)
        {
        }
        public static void CreateFiltredLogRepresentation(DrawingContext dc, ILineItem model)
        {
            Point pt = new Point(0,0);
            Point pt2 = new Point(200,0);
            dc.DrawLine(new Pen(), pt, pt2);
           
            FormattedText ft = new FormattedText(model.TotalLineHeight.ToString(),
                                CultureInfo.GetCultureInfo("en-us"),
                                FlowDirection.LeftToRight,
                                new Typeface(new FontFamily("Consolas"),
                                FontStyles.Normal,
                                FontWeights.Bold,
                                FontStretches.Normal),
                                frontSize,
                                Brushes.Black);
            pt.Y += frontSize;
            dc.DrawText(ft, pt);
            pt.Y += frontSize;
            pt2.Y += frontSize*2;
            dc.DrawLine(new Pen(), pt, pt2);
        }
        internal static void CreateSimpleLine(DrawingContext dc, Status status, LogLevel logLevel, String text,int lineHeight, CKTrait tag)
        {
            Point pt = new Point(0, 0);
            CreateSimpleLine(dc,status,logLevel,text, lineHeight, tag, pt);
                
        }
        internal static void CreateSimpleLine(DrawingContext dc, Status status, LogLevel logLevel, String text,int lineHeight, CKTrait tag, Point pt)
        {
            CreateExpander(dc, pt, status);
            pt.X += frontSize;
            CreateSymbol(dc, pt, logLevel);

            pt.X += frontSize;

            CreateContent(dc, pt, text, lineHeight);

            int logLovelContent = text.Length;
            pt.X += logLovelContent * frontSize;
            
            CreateTag(dc, pt, tag);
        }

        internal static void CreateException(DrawingContext dc, Status status, LogLevel logLevel, String text, int lineHeight, CKTrait tag, Exception exception)
        {
            Point pt = new Point(0, 0);
            CreateSimpleLine(dc, status, logLevel, text, lineHeight, tag, pt);

        }
    }
}
