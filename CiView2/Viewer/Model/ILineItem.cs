using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.View;

namespace Viewer.Model
{
    public interface ILineItem
    {
        ILineItemHost Host { get; }
        ILineItem Parent { get; }
        ILineItem Next { get; }
        ILineItem Prev { get; }
        ILineItem FirstChild { get; }
        ILineItem LastChild { get; }
        int Depth { get; }
        int AbsoluteY { get; }
        int TotalLineHeight { get; }
        int LineHeight { get; }
        void InsertChild( ILineItem child, ILineItem nextChild = null );
        void RemoveChild( ILineItem child );
        event EventHandler ChildInserted;
        VisualLineItem CreateVisualLine();
    }
}
