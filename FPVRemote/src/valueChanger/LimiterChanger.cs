using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPVRemote.valueChanger
{
    class LimiterChanger : AValueChanger
    {
        public int Min
        {
            get;
        }
        public int Max
        {
            get;
        }

        public LimiterChanger(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }

        protected override int ComputeImpl(int val, ChangerContext cc)
        {
            if (val < Min)
            {
                val = Min;
            }
            if (val > Max)
            {
                val = Max;
            }
            return val;
        }
    }
}
