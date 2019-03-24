using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPVRemote.valueChanger
{
    class MultiRangeChanger : AValueChanger
    {
        private MapRangeChanger[] rangeChangers;

        public MultiRangeChanger(MapRangeChanger[] rangeChangers)
        {
            this.rangeChangers = rangeChangers;
        }

        protected override int ComputeImpl(int val)
        {
            if (rangeChangers == null || rangeChangers.Length == 0)
            {
                return val;
            } else
            {
                int v = 0;
                foreach (MapRangeChanger mr in rangeChangers)
                {
                    
                    v = mr.ComputeValueDirectly(val);
                    if (val < mr.mapping.minFrom)
                    {
                        // too little, let it be
                        break;
                    }
                    if (val <= mr.mapping.maxTo)
                    {
                        // got it
                        break;
                    }
                }
                return v;
            }
        }
    }
}
