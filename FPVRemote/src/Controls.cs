using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using IniParser;
using IniParser.Model;


using FPVRemote.myRect;
using FPVRemote.valueChanger;


// joystick
using XInputDotNetPure;


namespace FPVRemote
{
    public partial class MainWindow : Window
    {
        // VARS -----------------------------------------------------------------

        MyRect centrR;
        MyRect bordrR;

        bool armed;

        short minSpeed;
        short maxSpeed;

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

            minSpeed = short.Parse(data["SPEED"]["min"]);
            maxSpeed = short.Parse(data["SPEED"]["max"]);           



            centrR = new MyRect(int.Parse(data["CENTER"]["x"]), int.Parse(data["CENTER"]["y"]), int.Parse(data["CENTER"]["w"]), int.Parse(data["CENTER"]["h"]));
            bordrR = new MyRect(int.Parse(data["BORDER"]["x"]), int.Parse(data["BORDER"]["y"]), int.Parse(data["BORDER"]["w"]), int.Parse(data["BORDER"]["h"]));

            armed = false;

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


            Point mouseLocation = GetMousePosition();
            int mX = (int)mouseLocation.X;
            int mY = (int)mouseLocation.Y;

            if (armed)
            {
                if (!bordrR.contains(mX, mY))
                {
                    results[CH1] = 127;
                    results[CH2] = 127;
                    armed = false;
                }
                else
                if (!centrR.contains(mX, mY))
                {
                    // steer ----------------------------------
                    if (true)
                    {
                        if (mX <= centrR.x)
                        {
                            results[CH1] = (short)(127 - (centrR.x - mX) * (127.0 / (centrR.x - bordrR.x)));
                        }
                        else if (mX > centrR.xMax) 
                        {
                            results[CH1] = (short)(127 + (mX - centrR.xMax) * (127.0 / (centrR.x - bordrR.x)));
                        }
                    }
                    // gas ------------------------------------
                    if (true)
                    {
                        if (mY <= centrR.y)
                        {
                            results[CH2] = (short)(127 + (centrR.y - mY) * (127.0 / (centrR.y - bordrR.y)));
                        }
                        else if (mY > centrR.yMax)
                        {
                            results[CH2] = (short)(127 - (mY - centrR.yMax) * (127.0 / (centrR.y - bordrR.y)));
                        }
                    }
                }
            } else
            {
                if (centrR.contains(mX, mY))
                {
                    armed = true;
                }
            }

            if (results[CH2] < minSpeed)
            {
                results[CH2] = minSpeed;
            }
            if (results[CH2] > maxSpeed)
            {
                results[CH2] = maxSpeed;
            }



            bool b1 = centrR.contains(mX, mY);
            bool b2 = bordrR.contains(mX, mY);


            //XAxisTextBox.Text = mouseLocation.X.ToString() + ", " + mouseLocation.Y.ToString() + "  : " + b.ToString();
            XAxisTextBox.Text = results[CH1].ToString() + ", " + results[CH2].ToString() + "  : (" + b1.ToString() + "  : " + b2.ToString() + ")  :: " + armed.ToString();
        }
    }
}
