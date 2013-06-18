using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    public interface ILineItemHost
    {
        ILineItem Root { get; }

        event EventHandler<LineItemChangedEventArgs> ItemChanged;
    }
}
