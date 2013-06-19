using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CK.Core;

namespace Randomizer
{
    public class RandomLogPlayer
    {
        RandomLog _random;
      
        public RandomLogPlayer()
        {
            _random = new RandomLog();
        }

        public RandomLog GetRandomLog()
        {
            return _random;
        }

        public void One(int maxDepth)
        {
            if (maxDepth < _random.GetDepth())
                _random.CloseStayingGroup(_random.GetDepth() - maxDepth);
            _random.GenerateOneLog(maxDepth);
        }
       
        public void Stop()
        {
            _random.CloseStayingGroup(_random.GetDepth());
        }

    }
}
