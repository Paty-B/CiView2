using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Viewer.Model;

namespace Viewer.View
{
    public static class VisualDesigner
    {
        public static void CreateExpander(DrawingContext drawingContext, Point drawingPosition, Status status)
        {
        }
        public static void CreateSymbol(DrawingContext drawingContext, Point drawingPosition, LogLevel logLevel)
        {
        }
        public static void CreateContent(DrawingContext drawingContext, Point drawingPosition, String content)
        {
            FormattedText ft = new FormattedText(
                                content, 
                                culture,   
                                FlowDirection.LeftToRight, 
                                new Typeface(new FontFamily("Consolas"), 
                                FontStyles.Normal, 
                                FontWeights.Bold, 
                                FontStretches.Normal),
                                24,
                                Brushes.Black);
            drawingContext.DrawText(ft, drawingPosition);
        }
        public static void CreateTag(DrawingContext drawingContext, Point drawingPosition, CKTrait tag)
        {
        }
        public static void CreateNextLogIndicator(DrawingContext drawingContext, Point drawingPosition, int nbWarning, int nbError, int nbFatal)
        {
        }
        public static void CreateFiltredLogRepresentation(DrawingContext drawingContext, Point drawingPosition)
        {
        }

        public static System.Globalization.CultureInfo culture { get; set; }
    }
}
