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
            internal CKTraitCheckboxInfo(string tagName)
            {
                CheckBoxObj = new CheckBox();
                Number = 0;

                CheckBoxObj.Uid = tagName;
                CheckBoxObj.IsChecked = true;
                Rise();
            }

            internal int Number { get; private set; }
            internal CheckBox CheckBoxObj { get; private set; }

            internal void Rise()
            {
                UpdateCheckBoxContent(1);
            }

            internal void decrease()
            {
                UpdateCheckBoxContent(-1);
            }

            private void UpdateCheckBoxContent(int value)
            {
                Number += value;
                CheckBoxObj.Content = CheckBoxObj.Uid + " (" + Number + ")";
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
            CKtraitRise(tags);
            CKtraitRise(tags);
            CKtraitRise(tags);
            CKtraitRise(tags);
            CKtraitRise(tags);
            tags = ActivityLogger.RegisteredTags.FindOrCreate("D|B");
            CKTraitDecrease(tags);
            CKTraitDecrease(tags);

            #endregion
        }


        public void CKtraitRise(CKTrait ckTrait)
        {
            UpdateCKTrait(ckTrait);
        }

        public void CKTraitDecrease(CKTrait ckTrait)
        {
            UpdateCKTrait(ckTrait, false);
        }

        private void UpdateCKTrait(CKTrait ckTrait, bool rise = true)
        {
            foreach (CKTrait trait in ckTrait.AtomicTraits)
            {
                string tagName = trait.ToString();
                CKTraitCheckboxInfo traitCBInfo;
                if (_traitsCBInfo.TryGetValue(tagName, out traitCBInfo))
                {
                    if (rise)
                        traitCBInfo.Rise();
                    else
                    {
                        traitCBInfo.decrease();
                        if (traitCBInfo.Number == 0)
                        {
                            _traitsCBInfo.Remove(tagName);
                            ListBoxTag.Items.Remove(traitCBInfo.CheckBoxObj);
                        }
                    }
                }
                else
                {
                    if (rise)
                    {
                        traitCBInfo = new CKTraitCheckboxInfo(tagName);
                        _traitsCBInfo.Add(tagName, traitCBInfo);
                        ListBoxTag.Items.Add(traitCBInfo.CheckBoxObj);
                        ListBoxTag.Items.SortDescriptions.Add(
                            new System.ComponentModel.SortDescription("Content",
                                System.ComponentModel.ListSortDirection.Ascending));
                    }
                }
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
