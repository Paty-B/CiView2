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

            _listBoxOfCheckBoxCounter.CheckBoxClick += CKTraitChecked;
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
                if (rise)
                    _listBoxOfCheckBoxCounter.Increase(tagName);
                else
                    _listBoxOfCheckBoxCounter.Decrease(tagName);
            }
        }

        private void CKTraitChecked(string uid, bool isChecked)
        {
            /*
            MessageBox.Show("CheckBox is " + (isChecked ? "checked " : "unchecked ") + uid
                    , "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            //*/
        }
    }
}
