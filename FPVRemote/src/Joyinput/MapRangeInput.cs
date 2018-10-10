using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPVRemote.Joyinput
{

    public struct RangeMapping
    {
        public int minFrom;
        public int maxFrom;
        public int minTo;
        public int maxTo;
    }


    public class MapRangeInput : AJoyInput
    {
        private RangeMapping mapping;
        private float factor;


        public MapRangeInput(RangeMapping mapping)
        {
            this.mapping = mapping;
            this.factor = (float)(this.mapping.maxTo - this.mapping.minTo) / (this.mapping.maxFrom - this.mapping.minFrom);
        }

        protected override int ComputeImpl(int val)
        {
            if (val < mapping.minFrom) {
                val = mapping.minFrom;
            }
            if (val > mapping.maxFrom)
            {
                val = mapping.maxFrom;
            }            
            return (int)(Math.Round(mapping.minTo + (Math.Abs(val - mapping.minFrom)) * factor));
        }
    }
}
