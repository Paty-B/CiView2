using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Viewer
{
    public class ListBoxOfCheckBoxCounter: ListBox
    {
        Dictionary<string, CheckBoxCounter> _dictionary;

        public delegate void EventCheckBoxClick(string uid, bool isChecked);

        public ListBoxOfCheckBoxCounter()
            : base()
        {
            _dictionary = new Dictionary<string, CheckBoxCounter>();
            ForceDelete = true;
            DefaultChecked = true;
            Sort = true;

            BorderThickness = new Thickness(0);
        }

        public bool ForceDelete { get; set; }
        public bool DefaultChecked { get; set; }
        public bool Sort { get; set; }
        public event EventCheckBoxClick CheckBoxClick;

        public void Increase(string uid)
        {
            CheckBoxCounter checkBoxCounter;
            if (!_dictionary.TryGetValue(uid, out checkBoxCounter))
                checkBoxCounter = InsertNewCheckBoxCounter(uid);
            checkBoxCounter.Counter++;
        }

        public void Decrease(string uid)
        {
            CheckBoxCounter checkBoxCounter;
            if (_dictionary.TryGetValue(uid, out checkBoxCounter))
                if(--checkBoxCounter.Counter == 0 && ForceDelete)
                    RemoveCheckBoxCounter(checkBoxCounter);
        }

        private CheckBoxCounter InsertNewCheckBoxCounter(string uid)
        {
            CheckBoxCounter checkBoxCounter = new CheckBoxCounter();
            checkBoxCounter.Uid = uid;
            checkBoxCounter.Click += CKTraitClick;
            checkBoxCounter.IsChecked = DefaultChecked;
            checkBoxCounter.Text = uid;
            Items.Add(checkBoxCounter);
            _dictionary.Add(uid, checkBoxCounter);
            Sorting();
            return checkBoxCounter;
        }

        private void RemoveCheckBoxCounter(CheckBoxCounter checkBoxCounter)
        {
            Items.Remove(checkBoxCounter);
            _dictionary.Remove(checkBoxCounter.Uid);
            Sorting();
        }

        public CheckBoxCounter Add(string uid)
        {
            return InsertNewCheckBoxCounter(uid);
        }

        private void CKTraitClick(object sender, RoutedEventArgs e)
        {
            CheckBoxCounter cb = (CheckBoxCounter)sender;
            bool isChecked = (bool)cb.IsChecked;
            string uid = cb.Uid;
            if (CheckBoxClick != null)
                CheckBoxClick(uid, isChecked);
        }

        private void Sorting()
        {
            if (Sort)
            {
                Items.SortDescriptions.Add(
                    new System.ComponentModel.SortDescription("Uid",
                        System.ComponentModel.ListSortDirection.Ascending));
            }
        }
    }
}
