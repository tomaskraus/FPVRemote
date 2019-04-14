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
        private int index;
        private int minThrottle;
        private int maxThrottle;
        int NumberOfCycles
        {
            get;
        }

        public ThrottleStatisticChanger(int cycles)
        {
            NumberOfCycles = cycles;
            values = new int[cycles];
            index = 0;
        }

        protected override int ComputeImpl(int val, ChangerContext cc)
        {
            values[index] = val;

            minThrottle = 256;
            maxThrottle = 0;
            foreach (int v in values) {
                if (v < minThrottle)
                {
                    minThrottle = v;
                }
                if (v > maxThrottle)
                {
                    maxThrottle = v;
                }
            }

            cc.minThrottleReached = minThrottle;
            cc.maxThrottleReached = maxThrottle;

            index = (index + 1) % NumberOfCycles;

            return val;
        }
    }
}
