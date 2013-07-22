using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{

    public enum LineItemChangedStatus
    {
        Visible,
        Invisible, 
        Inserted,
        Deleted, 
        Update
    }

    public class LineItemChangedEventArgs : EventArgs
    {
        public readonly ILineItem LineItem;
        public readonly LineItemChangedStatus Status;

        public LineItemChangedEventArgs(ILineItem line, LineItemChangedStatus status)
        {       
            LineItem = line;
            Status = status;
        }
    }
}
