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
        MyRect deadZoneR;

        bool armed;

        short minSpeed;
        short maxSpeed;

        const int CHsteer = 3;
        const int CHthrottle = 1;
        const int CHaux1 = 2;
        const int CHaux2 = 0;
        
        //joystick
        private GamePadState gPad1;

        IValueChanger chgSteer;
        InputChanger chgInputSteer;

        IValueChanger chgThrottle;
        InputChanger chgInputThrottle;

        RangeMapping gamepadRangeMapping;

        // INIT -----------------------------------------------------------------

        private void setArmed(bool armed)
        {
            if (armed)
            {
                this.armed = true;
                centr.Stroke = System.Windows.Media.Brushes.White;
            } else
            {
                this.armed = false;
                centr.Stroke = System.Windows.Media.Brushes.Red;
            }
        }


        public void initInputControls(IniData data, ref short[] initialResults)
        {
            for (int i = 0; i < initialResults.Length; i++)
            {
                initialResults[i] = 128;
            }

            //------------------------------------------------

            minSpeed = short.Parse(data["SPEED"]["min"]);
            maxSpeed = short.Parse(data["SPEED"]["max"]);

            centrR = new MyRect(0, 0, int.Parse(data["CENTER"]["w"]), int.Parse(data["CENTER"]["h"]));
            bordrR = new MyRect(int.Parse(data["BORDER"]["x"]), int.Parse(data["BORDER"]["y"]), int.Parse(data["BORDER"]["w"]), int.Parse(data["BORDER"]["h"]));

            deadZoneR = new MyRect(0, 0, bordrR.w - 2 * int.Parse(data["DEADZONE"]["x"]), bordrR.h - 2 * int.Parse(data["DEADZONE"]["y"]));

            setArmed(false);

            // TODO make configurable. Reason: poor quality of cheap gamepads
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
            chgInputThrottle.Input = (int)(gPad1.ThumbSticks.Right.Y * ushort.MaxValue);

            results[CHsteer] = (short)(chgSteer.ComputeValue());
            results[CHthrottle] = (short)(chgThrottle.ComputeValue());


            Point mouseLocation = GetMousePosition();
            Point mouseLocationBordr = bordr.PointFromScreen(mouseLocation);
            Point mouseLocationDeadzone = deadzone.PointFromScreen(mouseLocation);
            Point mouseLocationCentr = centr.PointFromScreen(mouseLocation);

            int mX = (int)mouseLocationDeadzone.X;
            int mY = (int)mouseLocationDeadzone.Y;

            //already assumed the center is really centered
            int centrMarginX = (int)((deadzone.Width - centr.Width) / 2);
            int centrMarginY = (int)((deadzone.Height - centr.Height) / 2);
            int centrMarginXEnd = (int)(centrMarginX + centr.Width);
            int centrMarginYEnd = (int)(centrMarginY + centr.Height);

            bool inBordr = bordr.RenderedGeometry.FillContains(mouseLocationBordr);
            bool inCentr = centr.RenderedGeometry.FillContains(mouseLocationCentr);


            if (armed)
            {
                if (!inBordr)
                {
                    results[CHsteer] = 127;
                    results[CHthrottle] = 127;
                    setArmed(false);
                    centr.Stroke = System.Windows.Media.Brushes.Red;
                }
                else
                if (!inCentr)
                {
                    // steer ----------------------------------
                    
                        if (mX <= centrMarginX)
                        {
                            results[CHsteer] = (short)(127 - (centrMarginX - mX) * (127.0 / centrMarginX));
                        }
                        else if (mX > centrMarginXEnd) 
                        {
                            results[CHsteer] = (short)(127 + (mX - centrMarginXEnd) * (127.0 / centrMarginX));
                        }

                    // gas ------------------------------------

                        if (mY <= centrMarginY)
                        {
                            results[CHthrottle] = (short)(127 + ((centrMarginY - mY) * ((double)(maxSpeed - 127) / (centrMarginY))));
                        }
                        else if (mY > centrMarginYEnd)
                        {
                            results[CHthrottle] = (short)(127 - ((mY - centrMarginYEnd) * ((double)(127 - minSpeed) / (centrMarginY))));
                        }
                    
                }
            } else
            {
                if (inCentr)
                {
                    setArmed(true);
                }
            }

            // ----- hard limits -------------------

            if (results[CHthrottle] < minSpeed)
            {
                results[CHthrottle] = minSpeed;
            }
            if (results[CHthrottle] > maxSpeed)
            {
                results[CHthrottle] = maxSpeed;
            }

            if (results[CHsteer] < 0)
            {
                results[CHsteer] = 0;
            }
            if (results[CHsteer] > 255)
            {
                results[CHsteer] = 255;
            }

            // -------------------------------------



            //XAxisTextBox.Text = mouseLocation.X.ToString() + ", " + mouseLocation.Y.ToString() + "  : " + b.ToString();
            XAxisTextBox.Text = gPad1.ThumbSticks.Left.X + ", " + gPad1.ThumbSticks.Right.Y
                + "\nst: " + results[CHsteer].ToString() + ", th: " + results[CHthrottle].ToString()
                + "\nc: " + inCentr.ToString() + "  : b: " + inBordr.ToString() + "\narmed: " + armed.ToString() + "\n"
                + "\n[" + mX.ToString() + ", " + mY.ToString() + "]"
                + "\n ml [" + mouseLocationBordr.X.ToString() + ", " + mouseLocationBordr.Y.ToString() + "]"
                ; 
                // + "\ncentrX=" + centrR.x;
                // + "\n" + (centrR.y - mY).ToString() + ":  " + ((double)maxSpeed / (centrR.y - bordrR.y)).ToString();
        }
    }
}
