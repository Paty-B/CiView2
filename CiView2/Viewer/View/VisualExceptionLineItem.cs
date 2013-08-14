using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Viewer.Model;

namespace Viewer.View
{
    class VisualExceptionLineItem : VisualLineItem
    {
        internal VisualExceptionLineItem(ExceptionLineItem model)
            :base(model)
        {
            DrawingContext dc = this.RenderOpen();

            VisualDesigner.CreateException(dc, model.Status, model.LogLevel, model.Content, model.LineHeight, model.Tag, model.exception);
            this.Offset = new Vector(0, 0);
        }

        internal override void OnClick(Visual target, Point inTarget)
        {
            throw new NotImplementedException();
        }
    }
}
