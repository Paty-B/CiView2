using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model.Events
{
    public class SizeScrollBarChangedEventArgs : EventArgs
    {
        public readonly double Size;

        public SizeScrollBarChangedEventArgs(double newSize)
        {
            Size = newSize;
        }
    }
}
