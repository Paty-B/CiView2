using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CK.Core;

namespace Viewer
{
    /// <summary>
    /// Logique d'interaction pour TagFilters.xaml
    /// </summary>
    public partial class TagFilters : UserControl
    {
        private Dictionary<string, CKTraitCheckboxInfo> _traitsCBInfo =
            new Dictionary<string, CKTraitCheckboxInfo>();

        private class CKTraitCheckboxInfo
        {
            internal CKTraitCheckboxInfo(CheckBox cb)
            {
                CheckBoxObj = cb;
                Number = 1;
            }
            internal int Number { get; private set; }
            internal CheckBox CheckBoxObj { get; private set; }
            internal int Increment() {
                return ++Number;
            }
        }

        public TagFilters()
        {
            InitializeComponent();

            #region faketags

            CKTrait tags = ActivityLogger.RegisteredTags.FindOrCreate("D|Z");
            CKtraitRise(tags);
            tags = ActivityLogger.RegisteredTags.FindOrCreate("Z|B");
            CKtraitRise(tags);
            tags = ActivityLogger.RegisteredTags.FindOrCreate("A|D");
            CKtraitRise(tags);
            tags = ActivityLogger.RegisteredTags.FindOrCreate("C");
            CKtraitRise(tags);
            tags = ActivityLogger.RegisteredTags.FindOrCreate("A|B|C");
            CKtraitRise(tags);
            tags = ActivityLogger.RegisteredTags.FindOrCreate("A|B|C");
            CKtraitRise(tags);
            tags = ActivityLogger.RegisteredTags.FindOrCreate("A|B|C");
            CKtraitRise(tags);
            tags = ActivityLogger.RegisteredTags.FindOrCreate("A|B|C");
            CKtraitRise(tags);

            #endregion
        }


        private void CKtraitRise(CKTrait ckTrait)
        {
            foreach (CKTrait trait in ckTrait.AtomicTraits)
                AddCheckbox(trait.ToString());
        }

        private void AddCheckbox(String traitName)
        {
            CKTraitCheckboxInfo traitCBInfo;
            if (_traitsCBInfo.TryGetValue(traitName, out traitCBInfo))
            {
                traitCBInfo.CheckBoxObj.Content = traitName + " (" + traitCBInfo.Increment() + ")";
            }
            else
            {
                traitCBInfo = new CKTraitCheckboxInfo(new CheckBox());
                _traitsCBInfo.Add(traitName, traitCBInfo);
                traitCBInfo.CheckBoxObj.Click += new RoutedEventHandler(CKTraitChecked);
                traitCBInfo.CheckBoxObj.Uid = traitName;
                traitCBInfo.CheckBoxObj.IsChecked = true;
                IEnumerable<KeyValuePair<string, CKTraitCheckboxInfo>> query =
                    _traitsCBInfo.OrderBy(tcbi => tcbi.Key);
                ListBoxTag.Items.Add(traitCBInfo.CheckBoxObj);
                traitCBInfo.CheckBoxObj.Content = traitName + " (1)";
                ListBoxTag.Items.SortDescriptions.Add(
                    new System.ComponentModel.SortDescription("Content",
                        System.ComponentModel.ListSortDirection.Ascending));
            }
        }

        private void CKTraitChecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            bool isChecked = (bool)cb.IsChecked;
            string tagName = cb.Uid;
            /*
            MessageBox.Show("CheckBox is " + (isChecked ? "checked " : "unchecked ") + tagName
                    , "Info", MessageBoxButton.OK, MessageBoxImage.Error);
             * */
        }
    }
}
