using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    interface ILineItemImpl : ILineItemParentImpl
    {
        new ILineItemParentImpl Parent { get; set; }
        new ILineItemImpl Next { get; set; }
        new ILineItemImpl Prev { get; set; }
        new ILineItemImpl FirstChild { get; }
        new ILineItemImpl LastChild { get; }
        void AdjustAbsoluteY( int delta );

        void Hidden();

        void unHidden();
    }
}
