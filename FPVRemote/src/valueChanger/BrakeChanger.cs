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


        protected override int ComputeImpl(int val)
        {
            switch (state)
            {
                case 0:
                    if (prevVal > Threshold && val <= Threshold)
                    {
                        state = 1;
                        val = MIN_VAL;
                        cnt = CntLimit;
                    }

                    break;

                case 1:
                    cnt--;
                    val = MIN_VAL;
                    if (cnt <= 1) {
                        val = Threshold;
                        prevVal = MIN_VAL;
                        state = 0;
                    }
                    break;
            }

            prevVal = val;

            return val;
        }
    }
}
