using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPVRemote.valueChanger
{
    public class ChangerContext
    {
        public int cycles
        {
            get; set;
        }

        public int minThrottleReached
        {
            get; set;
        }

        public int maxThrottleReached
        {
            get; set;
        }
    }
}
