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

        protected override void PollImpl()
        {
            if (vals.x < mapping.minFrom) {
                vals.x = mapping.minFrom;
            }
            if (vals.x > mapping.maxFrom)
            {
                vals.x = mapping.maxFrom;
            }

            //TODO do a rounding instead a cutoff
            vals.x = (int)(mapping.minTo + (Math.Abs(vals.x - mapping.minFrom)) * factor);
        }
    }
}
