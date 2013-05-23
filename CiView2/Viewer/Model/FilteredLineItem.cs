using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    public class FilteredLineItem : LineItemBase
    {
        int height;

        public override void InsertChild(LineItem child)
        {
            throw new NotImplementedException();
        }
    }
}
