using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using IniParser;
using IniParser.Model;

using FPVRemote.valueChanger;


// joystick
using XInputDotNetPure;


namespace FPVRemote
{
    public partial class MainWindow : Window
    {
        // VARS -----------------------------------------------------------------


        //joystick
        private GamePadState state;

        IValueChanger ji;
        InputChanger chgInputX;
        



        // INIT -----------------------------------------------------------------

        public void initInputControls(IniData data)
        {
            chgInputX = new InputChanger();
            ji = chgInputX
                .Chain(new MapRangeChanger(new RangeMapping
                {
                    minFrom = -65535,
                    maxFrom = 65535,
                    minTo = 0,
                    maxTo = 255
                }))
                ;

            
        }


        // LOOP -----------------------------------------------------------------

        public void loopInputControls(ref short[] results)
        {
            state = GamePad.GetState(PlayerIndex.One);
            int joyValX = (int)(state.ThumbSticks.Left.X * ushort.MaxValue);
            chgInputX.Input = joyValX;

            int v = ji.ComputeValue();

            results[0] = (short)v;

            XAxisTextBox.Text = v.ToString();
        }
    }
}
