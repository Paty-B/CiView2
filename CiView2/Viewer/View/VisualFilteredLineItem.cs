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
            DrawingContext dc = this.RenderOpen();
            VisualDesigner.CreateFiltredLogRepresentation(dc, model);
            dc.Close();
            this.Offset = new Vector(0, model.AbsoluteY*25);
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
