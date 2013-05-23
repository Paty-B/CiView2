using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    interface ILineItemParentImpl : ILineItem
    {
        new LineItemHost Host { get; }
        void Grow( int delta );
    }
}
