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
                    bi = ImageRequest.Instance.GetImage("expander2.ico");
                    break;
                case Status.Expanded:
                    bi = ImageRequest.Instance.GetImage("expander.ico");
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
        public static void CreateNextImportantLogsIndicator(DrawingContext dc, Point pt, int warn, int error, int fatal)
        {
            if (warn != 0)
            {
                CreateSymbol(dc, pt, LogLevel.Warn);
                pt.X += frontSize;
                CreateContent(dc, pt, warn.ToString(), 1);
                pt.X += frontSize;
            }
            if (error != 0)
            {
                CreateSymbol(dc, pt, LogLevel.Error);
                pt.X += frontSize;
                CreateContent(dc, pt, error.ToString(), 1);
                pt.X += frontSize;
            }
            if (fatal != 0)
            {
                CreateSymbol(dc, pt, LogLevel.Fatal);
                pt.X += frontSize;
                CreateContent(dc, pt, fatal.ToString(), 1);
                pt.X += frontSize;
            }          
                        
        }

        public static int CreateFiltredLogRepresentation(DrawingContext dc, ILineItem model)
        {
            Point pt = new Point(0, 0);
            Point pt2 = new Point(10, 0);
            dc.DrawLine(new Pen(new SolidColorBrush(), 10), pt, pt2);
           
            FormattedText ft = new FormattedText(model.TotalLineHeight.ToString(),
                                CultureInfo.GetCultureInfo("en-us"),
                                FlowDirection.LeftToRight,
                                new Typeface(new FontFamily("Consolas"),
                                FontStyles.Normal,
                                FontWeights.Bold,
                                FontStretches.Normal),
                                frontSize,
                                Brushes.Black);

            dc.DrawText(ft, pt);
            dc.DrawLine(new Pen(new SolidColorBrush(Color.FromRgb(0,0,0)), 2), pt, pt2);
            return model.TotalLineHeight;
        }

        public static int CreateFiltredLogRepresentation(DrawingContext dc, int nbLine)
        {
            Point pt = new Point(0, 0);
            Point pt2 = new Point(10, 0);
            dc.DrawLine(new Pen(new SolidColorBrush(), 10), pt, pt2);

            FormattedText ft = new FormattedText(nbLine.ToString(),
                                CultureInfo.GetCultureInfo("en-us"),
                                FlowDirection.LeftToRight,
                                new Typeface(new FontFamily("Consolas"),
                                FontStyles.Normal,
                                FontWeights.Bold,
                                FontStretches.Normal),
                                frontSize,
                                Brushes.Black);

            dc.DrawText(ft, pt);
            dc.DrawLine(new Pen(new SolidColorBrush(Color.FromRgb(0, 0, 0)), 2), pt, pt2);
            return nbLine;
        }

        public static void CreateInvisibleLog(DrawingContext dc)
        {
            Point pt = new Point(0, 0);
            FormattedText ft = new FormattedText("",
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

        internal static void CreateSimpleLine(DrawingContext dc, Status status, LogLevel logLevel, String text,int lineHeight, CKTrait tag)
        {
            Point pt = new Point(0, 0);
            CreateSimpleLine(dc,status,logLevel,text, lineHeight, tag, pt);
                
        }
        internal static void CreateSimpleLine(DrawingContext dc, Status status, LogLevel logLevel, String text,int lineHeight, CKTrait tag, Point pt)
        {
            if (status == Status.Hidden)
            {
            }
            else
            {
                //CreateExpander(dc, pt, status);
                pt.X += frontSize;
                CreateSymbol(dc, pt, logLevel);

                pt.X += frontSize;

                CreateContent(dc, pt, text, lineHeight);

                int logLovelContent = text.Length;
                pt.X += logLovelContent * frontSize;

                CreateTag(dc, pt, tag);
            }
        }

        internal static void CreateException(DrawingContext dc, Status status, LogLevel logLevel, String text, int lineHeight, CKTrait tag, Exception exception)
        {
            Point pt = new Point(0, 0);
            CreateSimpleLine(dc, status, logLevel, text, lineHeight, tag, pt);

        }


        internal static void CreateGroupeLine(DrawingContext dc, Status status, LogLevel logLevel, string text, int lineHeight, CKTrait tag, int warn, int error, int fatal)
        {
            Point pt = new Point(0, 0);

            if (status == Status.Hidden)
            {
            }
            else
            {
                CreateExpander(dc, pt, status);
                pt.X += frontSize;
                CreateSymbol(dc, pt, logLevel);

                pt.X += frontSize;

                CreateContent(dc, pt, text, lineHeight);

                int logLovelContent = text.Length;
                pt.X += logLovelContent * frontSize;

                CreateTag(dc, pt, tag);

                int tagLength = tag.ToString().Length;
                pt.X += tagLength * frontSize;

                CreateNextImportantLogsIndicator(dc, pt, warn, error, fatal);
            }
        }
    }
}
