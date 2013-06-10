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
    public abstract class VisualLineItem : ContainerVisual
    {
        readonly ILineItem _model;

        internal VisualLineItem(ILineItem model)
        {
            _model = model;
        }

        public ILineItem Model { get { return _model; } }

        internal abstract void OnClick(Visual target, Point inTarget);
    }
}
