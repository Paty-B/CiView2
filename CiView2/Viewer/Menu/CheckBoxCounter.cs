using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Viewer
{
    public class CheckBoxCounter : CheckBox
    {
        private int _counter = 0;
        private string _text;
        private string _stringFormat;

        public CheckBoxCounter()
            : base()
        {
            _stringFormat = "{0} ({1})";
            _text = "";
            update();

        }

        public int Counter
        {
            get
            {
                return _counter;
            }
            set
            {
                if (value == _counter)
                    return;
                _counter = value;
                update();
            }
        }

        public string StringFormat
        {
            get
            {
                return _stringFormat;
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException("StringFormat");
                if (value == _stringFormat)
                    return;
                _stringFormat = value;
                update();
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (value == null)
                    value = "";
                if (value == _text)
                    return;
                _text = value;
                update();
            }
        }

        private void update()
        {
            Content = string.Format(this.StringFormat, _text, _counter);
        }
    }
}
