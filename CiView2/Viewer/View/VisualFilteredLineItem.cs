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
    class VisualFilteredLineItem : VisualLineItem
    {
        internal VisualFilteredLineItem(ILineItem model)
            : base(model)
        {
            DrawingVisual dv = new DrawingVisual();
            DrawingContext dc = dv.RenderOpen();
            Point pt = new Point(model.Depth * 10, model.AbsoluteY);

            VisualDesigner.CreateFiltredLogRepresentation(dc, model, pt);
            dc.Close();
        }

        internal override void OnClick(Visual target, Point inTarget)
        {
            throw new NotImplementedException();
        }

        internal override void update()
        {
            throw new NotImplementedException();
        }
    }
}
