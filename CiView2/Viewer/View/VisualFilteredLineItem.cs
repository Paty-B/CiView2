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

            VisualDesigner.CreateFiltredLogRepresentation(dc, model);
            dc.Close();
        }

        internal override void OnClick(Visual target, Point inTarget)
        {
            throw new NotImplementedException();
        }
    }
}
