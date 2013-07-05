using CK.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.View;

namespace Viewer.Model
{
    abstract class LineItemBase : ILineItemImpl
    {
        ILineItemParentImpl _parent;
        ILineItemImpl _firstChild;
        ILineItemImpl _lastChild;
        ILineItemImpl _prevSibling;
        ILineItemImpl _nextSibling;
        int _absoluteY;
        int _totalHeight;
        int _lineHeight;


        internal LineItemBase()
        {
            _lineHeight = _totalHeight = 1;
        }
              

        ILineItemHost ILineItem.Host { get { return _parent != null ? _parent.Host : null; } }

        ILineItem ILineItem.Parent { get { return _parent; } }

        ILineItem ILineItem.Next { get { return _nextSibling; } }

        ILineItem ILineItem.Prev { get { return _prevSibling; } }

        ILineItem ILineItem.FirstChild { get { return _firstChild; } }

        ILineItem ILineItem.LastChild { get { return _lastChild; } }

        public LineItemHost Host { get { return _parent != null ? _parent.Host : null; } }

        public ILineItemParentImpl Parent 
        { 
            get { return _parent; } 
            set 
            { 
                _parent = value;
                if (_parent != null)
                {
                    _absoluteY = _prevSibling != null
                        ? _prevSibling.AbsoluteY + _prevSibling.TotalLineHeight
                        : _parent.AbsoluteY + _parent.TotalLineHeight;
                }
                else _absoluteY = 0;
            } 
        }

        public ILineItemImpl Next { get { return _nextSibling; } set { _nextSibling = value; } }

        public ILineItemImpl Prev { get { return _prevSibling; } set { _prevSibling = value; } }

        public ILineItemImpl FirstChild { get { return _firstChild; } }

        public ILineItemImpl LastChild { get { return _lastChild; } }

        public int Depth { get { return _parent == null ? -2 : _parent.Depth + 1; } }

        public int AbsoluteY { get { return _absoluteY;}}

        public int TotalLineHeight { get { return _totalHeight; } set { _totalHeight = value; } }

        public int LineHeight { get { return _lineHeight; }}

        public int FollowingNumberWarning { get; set; }
        public int FollowingNumberError { get; set; }
        public int FollowingNumberFatal { get; set; }
        public int FollowingNumberInfo { get; set; }
        public int FollowingNumberTrace { get; set; }

        public void RemoveChild( ILineItem child )
        {
            RemoveChild( child, this, ref _firstChild, ref _lastChild );
            Host.OnItemDeleted(child);
            if (child.GetType() != typeof(FilteredLineItem))
            {
                EventManager.Instance.OnRemoveChild(child, (LogLineItem)child);
            }
        }

        public void InsertChild( ILineItem child, ILineItem nextChild = null )
        {
            InsertChild(child, nextChild, this, ref _firstChild, ref _lastChild);
            Host.OnChildInserted(child);
            if (child.GetType() != typeof(FilteredLineItem))
            {
                EventManager.Instance.OnInsertChild(child, (LogLineItem)child);
            }
        }

        internal static void RemoveChild( ILineItem child, ILineItemParentImpl parent, ref ILineItemImpl firstChild, ref ILineItemImpl lastChild )
        {
            if( child == null ) throw new ArgumentNullException( "child" );
            if( child.Parent != parent ) throw new ArgumentException( "Parent mismatch.", "child" );

            ILineItemImpl c = (ILineItemImpl)child;
            parent.CountLogLevel(((LogLineItem)c).LogLevel, false);
            if( lastChild == c ) lastChild = c.Prev;
            else c.Next.Prev = c.Prev;
            if( firstChild == c ) firstChild = c.Next;
            else c.Prev.Next = c.Next;

            c.Prev = c.Next = null;
            c.Parent = null;
            parent.Grow( -c.TotalLineHeight );
        }

        internal static void InsertChild( ILineItem child, ILineItem nextChild, ILineItemParentImpl parent, ref ILineItemImpl firstChild, ref ILineItemImpl lastChild )
        {
            if( child == null ) throw new ArgumentNullException( "child" );
            if( nextChild != null && nextChild.Parent != parent ) throw new ArgumentException( "Parent mismatch.", "nextChild" );
            if( child.Parent != null ) child.Parent.RemoveChild( child );

            ILineItemImpl c = (ILineItemImpl)child;
            if( nextChild == null )
            {
                if( lastChild != null )
                {
                    Debug.Assert( lastChild.Next == null );
                    lastChild.Next = c;
                    c.Prev = lastChild;
                }
                if( firstChild == null ) firstChild = c;
                lastChild = c;
            }
            else
            {
                ILineItemImpl nextC = (ILineItemImpl)nextChild;
                Debug.Assert( firstChild != null && lastChild != null );
                c.Next = nextC;
                c.Prev = nextC.Prev;
                nextC.Prev = c;
                if( firstChild == nextC ) firstChild = c;
            }
            c.Parent = parent;
            parent.Grow( c.TotalLineHeight );
        }

        public void CountLogLevel(LogLevel loglevel,bool add)
        {
            switch (loglevel)
            {
                case LogLevel.Error:
                    if (add)
                       this.FollowingNumberError++;
                    else
                       this.FollowingNumberError--;
                    break;
                case LogLevel.Warn:
                    if(add)
                        this.FollowingNumberWarning++;
                    else
                        this.FollowingNumberWarning--;
                    break;
                case LogLevel.Fatal:
                    if(add)
                        this.FollowingNumberFatal++;
                    else
                        this.FollowingNumberFatal--;
                    break;
                case LogLevel.Trace:
                    if (add)
                        this.FollowingNumberTrace++;
                    else
                        this.FollowingNumberTrace--;
                    break;
                case LogLevel.Info:
                    if (add)
                        this.FollowingNumberInfo++;
                    else
                        this.FollowingNumberInfo--;
                    break;
                default:
                    break;
            }
            if (_parent != null) _parent.CountLogLevel(loglevel,add);
        }
        

        public void Grow( int delta )
        {
            _totalHeight += delta;
            var next = _nextSibling;
            while( next != null )
            {
                
                next.AdjustAbsoluteY( delta );
                Host.OnPositionChange(next);
                if (next.FirstChild != null)
                    next = next.FirstChild;
                else
                    next = next.Next;
            }
            if( _parent != null ) _parent.Grow( delta );
        }

        public void AdjustAbsoluteY( int delta )
        {
            _absoluteY += delta;
        }

        public abstract VisualLineItem CreateVisualLine();


        public abstract void Hidden();

        public abstract void unHidden();
    }
}
