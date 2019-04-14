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

        public int CntLimit
        {
            get;
        }

        private int prevVal;
        private int state;
        private int cnt;
        const int MIN_VAL = 10; 


        public BrakeChanger(int threshold, int cntLimit)
        {
            this.Threshold = threshold;
            this.CntLimit = cntLimit;
            prevVal = MIN_VAL;
            state = 0;
        }


        protected override int ComputeImpl(int val, ChangerContext cc)
        {
            int newVal = val;
            switch (state)
            {
                case 0:
                    if (prevVal > Threshold && newVal <= Threshold)
                    {
                        state = 1;
                        cnt = CntLimit;
                    }

                    break;

                case 1:
                    cnt--;
                    newVal = MIN_VAL;
                    if (cnt <= 1) {
                        newVal = Threshold;
                        prevVal = MIN_VAL;
                        state = 0;
                    }
                    break;
            }

            prevVal = newVal;

            return newVal;
        }
    }
}
