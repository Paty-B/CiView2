using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    class LineItemHost : ILineItemHost
    {
        internal readonly LineItemRoot Root;

        #region fast working method

        private Dictionary<string, List<ILineItem>> _indexILineItemByTag;
        private Dictionary<LogLevel, List<ILineItem>> _indexILineItemByLogLevel;
        private int lineItemCount = 0;

        #endregion

        public LineItemHost()
        {
            Root = new LineItemRoot(this);

            #region fast working method

            _indexILineItemByTag = new Dictionary<string, List<ILineItem>>();
            _indexILineItemByLogLevel = new Dictionary<LogLevel, List<ILineItem>>();

            EventManager.Instance.CheckBoxFilterTagClick += CheckBoxFilterTagClick;
            #endregion

        }

        ILineItem ILineItemHost.Root
        {
            get { return Root; }
        }

        public event EventHandler<LineItemChangedEventArgs> ItemChanged;

        internal void OnCollapsed(ILineItem item)
        {
            var h = ItemChanged;
            if (h != null) h(this, new LineItemChangedEventArgs(item, LineItemChangedStatus.Visible));
        }

        internal void OnExpended(ILineItem item)
        {
            var h = ItemChanged;
            if (h != null) h(this, new LineItemChangedEventArgs(item, LineItemChangedStatus.Visible));
        }

        internal void OnHiddened(ILineItem item)
        {
            var h = ItemChanged;
            if (h != null) h(this, new LineItemChangedEventArgs(item, LineItemChangedStatus.Invisible));
        }

        internal void OnPositionChange(ILineItem item)
        {
            var h = ItemChanged;
            if (h != null) h(this, new LineItemChangedEventArgs(item, LineItemChangedStatus.Update));
        }


        internal void OnItemDeleted(ILineItem item)
        {
            //*
            List<ILineItem> items;
            string tagName;

            #region unindex tag

            foreach (CKTrait tag in ((LogLineItem)item).Tag.AtomicTraits)
            {
                tagName = tag.ToString();
                if (_indexILineItemByTag.TryGetValue(tagName, out items))
                {
                    items.Remove(item);
                    if (items.Count == 0)
                        _indexILineItemByTag.Remove(tagName);
                }
            }

            #endregion

            #region unindex log level

            if (_indexILineItemByLogLevel.TryGetValue(((LogLineItem)item).LogLevel, out items))
            {
                items.Remove(item);
                if (items.Count == 0)
                    _indexILineItemByLogLevel.Remove(((LogLineItem)item).LogLevel);
            }

            #endregion
           //*/
            lineItemCount--;

            var h = ItemChanged;
            if (h != null) h(this, new LineItemChangedEventArgs(item, LineItemChangedStatus.Deleted));
        }

        internal void OnChildInserted(ILineItem inserted)
        {
            if (inserted!=null)
            {
                //*
                List<ILineItem> items;
                string tagName;

                #region index tag

                foreach (CKTrait tag in ((LogLineItem)inserted).Tag.AtomicTraits)
                {
                    tagName = tag.ToString();
                    if (!_indexILineItemByTag.TryGetValue(tagName, out items))
                    {
                        items = new List<ILineItem>();
                        _indexILineItemByTag.Add(tagName, items);
                    }
                    items.Add(inserted);
                }

                #endregion

                #region index log level

                if (!_indexILineItemByLogLevel.TryGetValue(((LogLineItem)inserted).LogLevel, out items))
                {
                    items = new List<ILineItem>();
                    _indexILineItemByLogLevel.Add(((LogLineItem)inserted).LogLevel, items);
                }

                items.Add(inserted);

                #endregion
                //*/
                lineItemCount++;
            }

            var h = ItemChanged;
            if (h != null) h(this, new LineItemChangedEventArgs(inserted, LineItemChangedStatus.Visible));
        }

        internal void OnFiltered(ILineItem item)
        {
            var h = ItemChanged;
            if (h != null) h(this, new LineItemChangedEventArgs(item, LineItemChangedStatus.Filtered));
        }

        internal void OnUnfiltered(ILineItem item)
        {
            var h = ItemChanged;
            if (h != null) h(this, new LineItemChangedEventArgs(item, LineItemChangedStatus.Visible));
        }

        private void CheckBoxFilterTagClick(string uid, bool isChecked)
        {
            #region
            List<ILineItem> items;
            if (_indexILineItemByTag.TryGetValue(uid, out items))
            {
                foreach (ILineItem item in items)
                {
                    if (!isChecked)
                    {
                        //item.Parent.InsertChild(new FilteredLineItem());
                    }
                }
            }
            #endregion
        }
    }
}

