﻿using System;
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
     
            DrawingContext dc = this.RenderOpen();

            VisualDesigner.CreateGroupeLine(dc, model.Status, model.LogLevel, model.Content,model.LineHeight, model.Tag, model.FollowingNumberWarning, model.FollowingNumberError, model.FollowingNumberFatal);
            dc.Close();
            this.Offset = new Vector(model.Depth*14, model.AbsoluteY*14);
        }
        internal override void OnClick(Visual target, Point inTarget)
        {
            throw new NotImplementedException();
        }
    }
}
