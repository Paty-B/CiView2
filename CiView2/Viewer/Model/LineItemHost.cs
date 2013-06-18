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

        public event EventHandler<LineItemChangedEventArgs> ItemChanged;
        
        internal void OnCollapsed( ILineItem item )
        {
            var h = ItemChanged;
            if (h != null) h( this, new LineItemChangedEventArgs(item,LineItemChangedStatus.Collapsed) );
        }
        
        internal void OnExpended(ILineItem item)
        {
            var h = ItemChanged;
            if (h != null) h( this, new LineItemChangedEventArgs(item,LineItemChangedStatus.Expanded) );
        }

        internal void OnItemDeleted(ILineItem item)
        {
            var h = ItemChanged;
            if (h != null) h( this, new LineItemChangedEventArgs(item,LineItemChangedStatus.Deleted) );
        }

        internal void OnChildInserted(ILineItem inserted )
        {
            var h = ItemChanged;
            if (h != null) h( this, new LineItemChangedEventArgs(inserted,LineItemChangedStatus.Inserted) );
        }

    }
}
