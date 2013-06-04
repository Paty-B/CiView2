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

        #region event

        public delegate void HiddenEventHandler(ILineItem sender, EventArgs e);
        public event HiddenEventHandler ItemHidden;
        internal void OnItemHidden(ILineItem item, EventArgs e)
        {
            // item caché
        }

        public delegate void CollaspedEventHandler(ILineItem sender, EventArgs e);
        public event CollaspedEventHandler ItemCollasped;
        internal void OnCollasped(ILineItem item, EventArgs e)
        {
            // item collapse
        }

        public delegate void ExpandedEventHandler(ILineItem sender, EventArgs e);
        public event ExpandedEventHandler ItemExpanded;
        internal void OnExpended(ILineItem item, EventArgs e)
        {
            //expand
        }

        public delegate void ChildInsertedEventHandler(ILineItem sender, EventArgs e);
        public event ChildInsertedEventHandler ChildInserted;
        internal void OnChildInserted(ILineItem child, EventArgs e)
        {
            if (ChildInserted != null)
                ChildInserted(child, e);
        }

        public delegate void ItemDeletedEventHandler(ILineItem sender, EventArgs e);
        public event ItemDeletedEventHandler ItemDeleted;
        internal void OnItemDeleted(ILineItem sender, EventArgs e)
        {
            if (ItemDeleted != null)
                ItemDeleted(sender, e);
        }


        #endregion
    }
}
