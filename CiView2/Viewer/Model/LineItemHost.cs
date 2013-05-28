using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    public delegate void ChildInsertedEventHandler(ILineItem sender, EventArgs e);

    class LineItemHost : ILineItemHost
    {
        internal readonly LineItemRoot Root;
        public event ChildInsertedEventHandler ChildInserted;

        public LineItemHost()
        {
            Root = new LineItemRoot( this );
        }

        ILineItem ILineItemHost.Root
        {
            get { return Root; }
        }

        internal virtual void OnChildInserted(LineItem sender, EventArgs e)
        {
            
        }



        internal void OnChildInserted(ILineItem child, EventArgs eventArgs)
        {
            if (ChildInserted != null)
                ChildInserted(child, eventArgs);
        }
    }
}
