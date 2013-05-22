using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    class RootLineItem : LineItem
    {
        public LineItemHost Host { get; internal set; }

        public RootLineItem(LineItemHost host)
        {
            Host = host;
            
        }

        public override void InsertChild(LineItem child)
        {
            Contract.Requires(child != null, "LineItem child must be not null");
            child.Parent = this;
            child.Depth = Depth + 1;

            if (FirstChild == null)
            {
                FirstChild = child;
                LastChild = child;
            }
            else
            {
                child.Previous = LastChild;
                LastChild.Next = child;
                LastChild = child;
            }
        }
    }
}
