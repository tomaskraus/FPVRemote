using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPVRemote.valueChanger
{
    class ThrottleStatisticChanger : AValueChanger
    {
        private int[] values;
        private int minThrottle;
        private int maxThrottle;
        int NumberOfCycles
        {
            get;
        }

        public ThrottleStatisticChanger(int cycles)
        {
            this.NumberOfCycles = cycles;
            this.values = new int[cycles];
            this.minThrottle = 256;
            this.maxThrottle = 0;
        }

        protected override int ComputeImpl(int val, ChangerContext cc)
        {

            if (val < minThrottle)
            {
                minThrottle = val;
            }
            if (val > maxThrottle)
            {
                maxThrottle = val;
            }
            cc.minThrottle = minThrottle;
            cc.maxThrottle = maxThrottle;
            return val;
        }
    }
}
