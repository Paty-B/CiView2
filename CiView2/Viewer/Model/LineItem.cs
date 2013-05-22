using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    public interface ILineItem
    {
        ILineItemHost Host { get; }
        ILineItem Parent { get; }
        ILineItem Next { get; }
        ILineItem Prev { get; }
        ILineItem FirstChild { get; }
        ILineItem LastChild { get; }
        int Depth { get; }
        int AbsoluteY { get; }
        int TotalLineHeight { get; }
        void InsertChild( ILineItem child, ILineItem nextChild = null );
        void RemoveChild( ILineItem child );
        VisualLine CreateLine();
    }

    public interface ILineItemHost
    {
        ILineItem Root { get; }
    }

    interface ILineItemParentImpl : ILineItem
    {
        new LineItemHost2 Host { get; }
        void Grow( int delta );
    }

    interface ILineItemImpl : ILineItemParentImpl
    {
        new ILineItemParentImpl Parent { get; set; }
        new ILineItemImpl Next { get; set; }
        new ILineItemImpl Prev { get; set; }
        new ILineItemImpl FirstChild { get; }
        new ILineItemImpl LastChild { get; }
        void AdjustAbsoluteY( int delta );
    }

    class LineItemHost2 : ILineItemHost
    {
        internal readonly LineItemRoot Root;

        public LineItemHost2()
        {
            Root = new LineItemRoot( this );
        }

        ILineItem ILineItemHost.Root
        {
            get { return Root; }
        }

    }



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

        public abstract VisualLine CreateLine();

        ILineItemHost ILineItem.Host { get { return _parent != null ? _parent.Host : null; } }

        ILineItem ILineItem.Parent { get { return _parent; } }

        ILineItem ILineItem.Next { get { return _nextSibling; } }

        ILineItem ILineItem.Prev { get { return _prevSibling; } }

        ILineItem ILineItem.FirstChild { get { return _firstChild; } }

        ILineItem ILineItem.LastChild { get { return _lastChild; } }

        public LineItemHost2 Host { get { return _parent != null ? _parent.Host : null; } }

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
        }

        public void InsertChild( ILineItem child, ILineItem nextChild = null )
        {
            InsertChild( child, nextChild, this, ref _firstChild, ref _lastChild );
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

    }

    class LogLineItem : LineItemBase
    {

    }

    static public class LineItem
    {
        public static ILineItem CreateLogLineItem()
        {
            return new LogLineItem();
        }
    }

    class LineItemRoot : ILineItemParentImpl
    {
        readonly LineItemHost2 _host;
        ILineItemImpl _firstChild;
        ILineItemImpl _lastChild;
        int _totalHeight;

        internal LineItemRoot( LineItemHost2 host )
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

        public int TotalLineHeight
        {
            get { return _totalHeight; }
        }

        public LineItemHost2 Host
        {
            get { return _host; }
        }

        public void Grow( int delta )
        {
            _totalHeight += delta;
        }

        public void InsertChild( ILineItem child, ILineItem nextChild = null )
        {
            LineItemBase.InsertChild( child, nextChild, this, ref _firstChild, ref _lastChild );
        }

        public void RemoveChild( ILineItem child )
        {
            LineItemBase.RemoveChild( child, this, ref _firstChild, ref _lastChild );
        }

    }


    // --------------------------------------------------------------

    
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
