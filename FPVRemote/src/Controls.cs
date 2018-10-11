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

        const int CH1 = 0;
        const int CH2 = 1;
        const int CH3 = 2;
        const int CH4 = 3;

        //joystick
        private GamePadState gPad1;

        IValueChanger chgSteer;
        InputChanger chgInputSteer;

        IValueChanger chgThrottle;
        InputChanger chgInputThrottle;

        RangeMapping gamepadRangeMapping;

        // INIT -----------------------------------------------------------------

        public void initInputControls(IniData data, ref short[] initialResults)
        {
            for (int i = 0; i < initialResults.Length; i++)
            {
                initialResults[i] = 128;
            }

            //------------------------------------------------

            gamepadRangeMapping = new RangeMapping
            {
                minFrom = -65535,
                maxFrom = 65535,
                minTo = 0,
                maxTo = 255
            };

            chgInputSteer = new InputChanger();
            chgSteer = chgInputSteer
                .Chain(new MapRangeChanger(gamepadRangeMapping))
                ;
            chgInputThrottle = new InputChanger();
            chgThrottle = chgInputThrottle
                .Chain(new MapRangeChanger(gamepadRangeMapping))
                ;

        }


        // LOOP -----------------------------------------------------------------

        public void loopInputControls(ref short[] results)
        {
            gPad1 = GamePad.GetState(PlayerIndex.One);
            chgInputSteer.Input = (int)(gPad1.ThumbSticks.Left.X * ushort.MaxValue);
            chgInputThrottle.Input = (int)((gPad1.Triggers.Right - gPad1.Triggers.Left) * ushort.MaxValue);

            results[CH1] = (short)(chgSteer.ComputeValue());
            results[CH2] = (short)(chgThrottle.ComputeValue());


            XAxisTextBox.Text = results[CH1].ToString();
        }
    }
}
