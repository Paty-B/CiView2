using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.View;

namespace Viewer.Model
{
    class FilteredLineItem : LineItemBase
    {

        public override VisualLineItem CreateVisualLine()
        {

            VisualFilteredLineItem Vl = new VisualFilteredLineItem(this);

            return Vl;
        }

        public override void Hidden()
        {
            throw new NotImplementedException();
        }

        public override void unHidden()
        {
            throw new NotImplementedException();
        }
    }
}
