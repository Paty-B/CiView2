using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;

namespace Viewer.Model
{
    class LineItemRoot : ILineItemParentImpl
    {  
        readonly LineItemHost _host;
        ILineItemImpl _firstChild;
        ILineItemImpl _lastChild;
        int _totalHeight;

        internal LineItemRoot( LineItemHost host )
        {
            _host = host;
        }

        ILineItemHost ILineItem.Host { get { return _host; } }

        public ILineItem Parent
        {
            get { return null; }
        }

        public ILineItem Next
        {
            get { return null; }
        }

        public ILineItem Prev
        {
            get { return null; }
        }

        public ILineItem FirstChild
        {
            get { return _firstChild; }
        }

        public ILineItem LastChild
        {
            get { return _lastChild; }
        }

        public int Depth
        {
            get { return -1; }
        }

        public int AbsoluteY
        {
            get { return 0; }
        }

        public int LineHeight
        {
            get { return 0; }
        }

        int ILineItem.TotalLineHeight
        {
            get
            {
                return _totalHeight;
            }
            set
            {
                _totalHeight = value;
            }
        }

        public LineItemHost Host
        {
            get { return _host; }
        }

        public int followingNumberWarning;
        public int followingNumberError;
        public int followingNumberFatal;
        public int followingNumberInfo;
        public int followingNumberTrace;



        public void Grow( int delta )
        {
            _totalHeight += delta;
        }

        void CountLogLevel(LogLevel loglevel, bool add)
        {
            switch (loglevel)
            {
                case LogLevel.Error:
                    if (add)
                        this.followingNumberError++;
                    else
                        this.followingNumberError--;
                    break;
                case LogLevel.Warn:
                    if (add)
                        this.followingNumberWarning++;
                    else
                        this.followingNumberWarning--;
                    break;
                case LogLevel.Fatal:
                    if (add)
                        this.followingNumberFatal++;
                    else
                        this.followingNumberFatal--;
                    break;
                case LogLevel.Trace:
                    if (add)
                        this.followingNumberTrace++;
                    else
                        this.followingNumberTrace--;
                    break;
                case LogLevel.Info:
                    if (add)
                        this.followingNumberInfo++;
                    else
                        this.followingNumberInfo--;
                    break;
                default:
                    break;
            }
        }

        public void InsertChild( ILineItem child, ILineItem nextChild = null )
        {
            LineItemBase.InsertChild( child, nextChild, this, ref _firstChild, ref _lastChild );
            Host.OnChildInserted(child);
            if (child != null)
            {
                EventManager.Instance.OnInsertChild(child, (LogLineItem)child);
            }
        }

        public void RemoveChild( ILineItem child )
        {
            LineItemBase.RemoveChild( child, this, ref _firstChild, ref _lastChild );
            Host.OnItemDeleted(child);
            if (child != null)
            {
                EventManager.Instance.OnRemoveChild(child, (LogLineItem)child);
            }
        }

        public View.VisualLineItem CreateVisualLine()
        {
            throw new System.ArgumentException("do not create a visual line for the root line");
        }


        void ILineItemParentImpl.CountLogLevel(LogLevel loglevel, bool add)
        {
            CountLogLevel(loglevel, add);
        }       
    }
}
