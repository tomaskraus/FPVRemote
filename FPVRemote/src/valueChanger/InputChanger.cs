using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPVRemote.valueChanger
{
    class InputChanger : AValueChanger
    {
        private int input;

        public int Input
        {
            set { input = value; }
        }


        public InputChanger() { }
      

        protected override int ComputeImpl(int val, ChangerContext cc)
        {
            return input;
        }
    }
}

