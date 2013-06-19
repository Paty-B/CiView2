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

        public delegate void EventInsertChild(ILineItem itemImpl, LogLineItem item);
        public event EventInsertChild InsertChild;
        public void OnInsertChild(ILineItem itemImpl, LogLineItem item)
        {
            if (InsertChild != null)
                InsertChild(itemImpl, item);
        }

        public delegate void EventRemoveChild(ILineItem itemImpl, LogLineItem item);
        public event EventRemoveChild RemoveChild;
        public void OnRemoveChild(ILineItem itemImpl, LogLineItem item)
        {
            if (RemoveChild != null)
                RemoveChild(itemImpl, item);
        }

        public delegate void EventCheckBoxFilterTagClick(string uid, bool isChecked);
        public event EventCheckBoxFilterTagClick CheckBoxFilterTagClick;
        public void OnCheckBoxFilterTagClick(string uid, bool isChecked)
        {
            if (CheckBoxFilterTagClick != null)
                CheckBoxFilterTagClick(uid, isChecked);
        }

        public delegate void EventCheckBoxFilterLogLevelClick(string uid, bool isChecked);
        public event EventCheckBoxFilterLogLevelClick CheckBoxFilterLogLevelClick;
        public void OnCheckBoxFilterLogLevelClick(string uid, bool isChecked)
        {
            if (CheckBoxFilterLogLevelClick != null)
                CheckBoxFilterLogLevelClick(uid, isChecked);
        }
    }
}
