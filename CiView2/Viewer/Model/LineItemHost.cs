using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    class LineItemHost : ILineItemHost
    {
       internal readonly LineItemRoot Root;

        public LineItemHost()
        {
            Root = new LineItemRoot( this );
        }

        ILineItem ILineItemHost.Root
        {
            get { return Root; }
        }
    }
}
