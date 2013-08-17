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
        int linesFiltered;
        internal VisualFilteredLineItem(ILineItem model)
            : base(model)
        {
            DrawingContext dc = this.RenderOpen();
            linesFiltered = VisualDesigner.CreateFiltredLogRepresentation(dc, model);
            dc.Close();
            this.Offset = new Vector(0, 0);
            
        }

        internal VisualFilteredLineItem(ILineItem model, int nb)
            : base(model)
        {
            DrawingContext dc = this.RenderOpen();
            linesFiltered = VisualDesigner.CreateFiltredLogRepresentation(dc, nb);
            dc.Close();
            this.Offset = new Vector(0, 0);
        }

        public int GetLinesFiltered()
        {
            return linesFiltered;
        }

        public void HideALine()
        {
            DrawingContext dc = this.RenderOpen();
            VisualDesigner.CreateInvisibleLog(dc);
            dc.Close();
        }

        internal override void OnClick(Visual target, Point inTarget)
        {
            throw new NotImplementedException();
        }
    }
}
