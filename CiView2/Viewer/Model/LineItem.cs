using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    public abstract class LineItem
    {
        #region Properties
        //public LineItemHost Host { get; internal set; }
        public LineItem Parent { get; internal set; }
        public LineItem Next { get; internal set; }
        public LineItem FirstChild { get; internal set; }
        public LineItem LastChild { get; internal set; }
        public LineItem Previous { get; internal set; }
        public int Depth { get; internal set; }
        public int LineHeight { get; set; }
        public int TotalLineHeight { get; set; }
        public Status Status { get; internal set; }
        public int AbsoluteY;
        public int LocalY;
        public LineItem NextError;
        public int NbFatal;
        public int NbError;
        public int NbWarning;
        #endregion

        public LineItem()
        {
            Next = null;
            Previous = null;

        }

        public abstract void InsertChild(LineItem child);
    }
}
