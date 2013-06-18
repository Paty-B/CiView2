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
    class VisualGroupLineItem : VisualLineItem
    {
        internal VisualGroupLineItem(LogLineItem model)
            : base(model)
        {
            DrawingVisual dv = new DrawingVisual();
            DrawingContext dc = dv.RenderOpen();

            VisualDesigner.CreateSimpleLine(dc, model.Status, model.LogLevel, model.Content,model.LineHeight, model.Tag);
            VisualDesigner.CreateNextImportantLogsIndicator(dc,model);
        }
        internal override void OnClick(Visual target, Point inTarget)
        {
            throw new NotImplementedException();
        }
    }
}
