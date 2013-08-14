using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;

namespace Viewer.Model
{
    interface ILineItemParentImpl : ILineItem
    {
        new LineItemHost Host { get; }
        void CountLogLevel(LogLevel loglevel,bool add);
    }
}
