﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    public class BagItems
    {
        #region Properties
        public LineItem FirstChild { get; internal set; }
        public LineItem LastChild { get; internal set; }
        public int ChildrenNumber { get; set; }
        public int BagHeight;
        #endregion


        public void InsertRootItems(List<LineItem> items)
        {
        }

        public void InsterRootItem(LineItem item)
        {
        }


    }
}
