using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPVRemote.valueChanger
{
    class MultiRangeChanger : AValueChanger
    {
        private MapRangeChanger[] rangeChangers;

        public MultiRangeChanger(MapRangeChanger[] rangeChangers)
        {
            this.rangeChangers = rangeChangers;
        }


        public MultiRangeChanger(RangeMapping globalRange, int numOfPoints, int[] percents)
        {
            if (numOfPoints < 3)
            {
                throw new ArgumentOutOfRangeException("at least 3 points, is " + numOfPoints);
            }
            if (percents.Length != numOfPoints)
            {
                throw new ArgumentException("percents items [" + percents.Length + "] must be equal to numOfPoints [" + numOfPoints + "]");
            }

            int numOfZones = numOfPoints - 1;
            int h = globalRange.maxTo - globalRange.minTo;
            double stepFrom = (double)(globalRange.maxFrom - globalRange.minFrom) / numOfZones;
            double x1 = globalRange.minFrom;
            double x2 = x1 + stepFrom;
            
            rangeChangers = new MapRangeChanger[numOfZones];
            for (int i = 0; i < rangeChangers.Length; i++)
            {               
                RangeMapping rm = new RangeMapping
                {
                   minFrom = (int)Math.Round(x1),
                   maxFrom = (int)Math.Round(x2),
                   minTo = (int)Math.Round(globalRange.minTo + h * percents[i] / 100.0),
                   maxTo = (int)Math.Round(globalRange.minTo + h * percents[i + 1] / 100.0),
                };
                rangeChangers[i] = new MapRangeChanger(rm);
                x1 = x2;
                x2 = x2 + stepFrom;
            }
        }

        protected override int ComputeImpl(int val, ChangerContext cc)
        {
            if (rangeChangers == null || rangeChangers.Length == 0)
            {
                return val;
            } else
            {
                int v = 0;
                foreach (MapRangeChanger mr in rangeChangers)
                {
                    
                    v = mr.ComputeValueDirectly(val, cc);
                    if (val < mr.mapping.minFrom)
                    {
                        // too little, let it be
                        break;
                    }
                    if (val <= mr.mapping.maxFrom)
                    {
                        // got it
                        break;
                    }
                }
                return v;
            }
        }
    }
}
