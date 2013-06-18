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
    class VisualLogLineItem : VisualLineItem
    {
        internal VisualLogLineItem(LogLineItem model)
            : base(model)
        {
            DrawingContext dc = this.RenderOpen();
            Point position = new Point(model.Depth*10, model.AbsoluteY);

            VisualDesigner.CreateSimpleLine(dc, model.Status, model.LogLevel, model.Content, model.LineHeight, model.Tag, position);
            dc.Close();
        }

        internal override void OnClick(Visual target, Point inTarget)
        {
        }

        internal override void update()
        {

        }
    }
}
