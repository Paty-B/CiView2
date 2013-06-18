using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.Model;

namespace Viewer
{
    class EventManager
    {
        private static EventManager _intance;

        private EventManager() { }

        public static EventManager Instance
        {
            get
            {
                if (_intance == null)
                {
                    _intance = new EventManager();
                }
                return _intance;
            }
        }

        public delegate void EventInsertChild(ILineItemImpl itemImpl, LogLineItem item);
        public event EventInsertChild InsertChild;
        public void OnInsertChild(ILineItemImpl itemImpl, LogLineItem item)
        {
            if (InsertChild != null)
                InsertChild(itemImpl, item);
        }

        public delegate void EventRemoveChild(ILineItemImpl itemImpl, LogLineItem item);
        public event EventRemoveChild RemoveChild;
        public void OnRemoveChild(ILineItemImpl itemImpl, LogLineItem item)
        {
            if (RemoveChild != null)
                RemoveChild(itemImpl, item);
        }
    }
}
