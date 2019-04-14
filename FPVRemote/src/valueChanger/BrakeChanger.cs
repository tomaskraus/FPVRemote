using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPVRemote.valueChanger
{
    class BrakeChanger : AValueChanger
    {
        public int Threshold
        {
            get;
        }
        public int PreviousMotionThreshold
        {
            get;
        }
        public int ReversePreviousMotionThreshold
        {
            get;
        }

        public int CntLimit
        {
            get;
        }

        private int prevVal;
        private int state;
        private int cnt;
        const int MIN_VAL = 10;
        const int MAX_VAL = 145;

        public BrakeChanger(int threshold, int previousMotionThreshold, int reversePreviousMotionThreshold, int cntLimit)
        {
            this.Threshold = threshold;
            this.PreviousMotionThreshold = previousMotionThreshold;
            this.ReversePreviousMotionThreshold = reversePreviousMotionThreshold;
            this.CntLimit = cntLimit;
            prevVal = Threshold;
            state = 0;
        }


        protected override int ComputeImpl(int val, ChangerContext cc)
        {
            int newVal = val;
            switch (state)
            {
                case 0:
                    if (prevVal > Threshold && newVal <= Threshold
                        && cc.maxThrottleReached >= PreviousMotionThreshold)
                    {
                        state = 1;
                        cnt = CntLimit;
                    }

                    if (prevVal < Threshold && newVal == Threshold
                        && cc.minThrottleReached <= ReversePreviousMotionThreshold)
                    {
                        state = -1;
                        cnt = CntLimit;
                    }

                    break;

                case 1:
                    cnt--;
                    newVal = MIN_VAL;
                    if (cnt < 1) {
                        newVal = Threshold;
                        state = 0;
                    }
                    break;

                case -1:
                    cnt--;
                    newVal = MAX_VAL;
                    if (cnt < 1)
                    {
                        cnt = CntLimit / 4;

                        state = 1;
                    }
                    break;

            }

            prevVal = newVal;

            return newVal;
        }
    }
}
