using FPVRemote.Joyinput;

using IniParser;
using IniParser.Model;

namespace FPVRemote.Joyinput
{

    /// <summary>
    /// Just the fake joystick, increases the x-value automatically
    /// </summary>
    public class CountJoyInput : AJoyInput
    {
        private MyCounter cnt;

        public CountJoyInput()
        {
            cnt = new MyCounter(0);
        }

        public CountJoyInput InitFromConfig(IniData configData, string key)
        {
            

            int counterStart = int.Parse(configData[key]["counterStart"]);
            cnt = new MyCounter(counterStart);

            return this;
        }

        
        protected override void PollImpl()
        {
            vals.x = cnt.Inc();
            vals.x = cnt.Inc();
            //vals.x = cnt.Inc();
            vals.y = 2;
        }
    }

    


    class MyCounter
    {
        public MyCounter(int value)
        {
            Val = value;
        }

        public int Inc()
        {
            this.Val++;
            return Val;
        }

        public int Val { get; private set; }
    }


    public class LimitJoyInput : AJoyInput
    {

        private int limit;

        public int Limit { get => limit; }

        public LimitJoyInput(int limit)
        {
            this.limit = limit;
        }


        protected override void PollImpl()
        {
            if (vals.x > limit) {
                vals.x = limit;
            }
        }
    }
}
