using CK.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal LineItemBase()
        {
        }

        //public abstract VisualLine CreateLine();

        ILineItemHost ILineItem.Host { get { return _parent != null ? _parent.Host : null; } }

        ILineItem ILineItem.Parent { get { return _parent; } }

        ILineItem ILineItem.Next { get { return _nextSibling; } }

        ILineItem ILineItem.Prev { get { return _prevSibling; } }

        ILineItem ILineItem.FirstChild { get { return _firstChild; } }

        ILineItem ILineItem.LastChild { get { return _lastChild; } }

        public LineItemHost Host { get { return _parent != null ? _parent.Host : null; } }

        public ILineItemParentImpl Parent { get { return _parent; } set { _parent = value; } }

        public ILineItemImpl Next { get { return _nextSibling; } set { _nextSibling = value; } }

        public ILineItemImpl Prev { get { return _prevSibling; } set { _prevSibling = value; } }

        public ILineItemImpl FirstChild { get { return _firstChild; } }

        public ILineItemImpl LastChild { get { return _lastChild; } }

        public int Depth { get { return _parent == null ? -2 : _parent.Depth + 1; } }

        public int AbsoluteY { get { return _absoluteY; } }

        public int TotalLineHeight { get { return _totalHeight; } }

        public void RemoveChild( ILineItem child )
        {
            RemoveChild( child, this, ref _firstChild, ref _lastChild );
            Host.OnItemDeleted(child, EventArgs.Empty);
        }

        public void InsertChild( ILineItem child, ILineItem nextChild = null )
        {
            InsertChild( child, nextChild, this, ref _firstChild, ref _lastChild );
            Host.OnChildInserted(child, EventArgs.Empty);
        }

        internal static void RemoveChild( ILineItem child, ILineItemParentImpl parent, ref ILineItemImpl firstChild, ref ILineItemImpl lastChild )
        {
            if( child == null ) throw new ArgumentNullException( "child" );
            if( child.Parent != parent ) throw new ArgumentException( "Parent mismatch.", "child" );

            ILineItemImpl c = (ILineItemImpl)child;
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

        public void Grow( int delta )
        {
            _totalHeight += delta;
            var next = _nextSibling;
            while( next != null )
            {
                next.AdjustAbsoluteY( delta );
                next = next.Next;
            }
            if( _parent != null ) _parent.Grow( delta );
        }

        public void AdjustAbsoluteY( int delta )
        {
            _absoluteY += delta;
        }

        public event EventHandler ChildInserted;
    }
}
