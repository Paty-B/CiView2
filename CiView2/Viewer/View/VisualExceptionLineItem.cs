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
            DrawingVisual dv = new DrawingVisual();
            DrawingContext dc = dv.RenderOpen();

            VisualDesigner.CreateException(dc, model.Status, model.LogLevel, model.Content, model.LineHeight, model.Tag, model.exception);
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
