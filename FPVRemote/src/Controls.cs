using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using IniParser;
using IniParser.Model;

using FPVRemote.valueChanger;
using FPVRemote.RCSender;

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
        SerialRCSender rcSender;



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

            rcSender = new SerialRCSender().InitFromConfig(data, "RC");
        }


        // LOOP -----------------------------------------------------------------

        public void loopInputControls()
        {
            state = GamePad.GetState(PlayerIndex.One);
            int joyValX = (int)(state.ThumbSticks.Left.X * ushort.MaxValue);
            chgInputX.Input = joyValX;

            int v = ji.ComputeValue();
            rcSender.Send(v.ToString() + "\n");

            XAxisTextBox.Text = v.ToString();
        }
    }
}
