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

        MultiRangeChanger throttleCurveChanger;
        MultiRangeChanger throttleLimitChanger;

        BrakeChanger brakeChanger;

        LimiterChanger ThrottleHardLimitChanger;
        LimiterChanger SteerHardLimitChanger;

        RangeMapping gamepadRangeMapping;

        ChangerContext chgContext;

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


        private void resetResults(ref short[] initialResults)
        {
            for (int i = 0; i < initialResults.Length; i++)
            {
                initialResults[i] = 127;
            }
        }

        public void initInputControls(IniData data, ref short[] initialResults)
        {
            chgContext = new ChangerContext();

            resetResults(ref initialResults);

            //------------------------------------------------


            throttleCurveChanger = new MultiRangeChanger(
                new RangeMapping { minFrom = 0, maxFrom = 255, minTo = 0, maxTo = 255 },
                7,
                new int[] {
                    // quick and ugly
                    int.Parse(data["THROTTLE"]["t0"]),
                    int.Parse(data["THROTTLE"]["t1"]),
                    int.Parse(data["THROTTLE"]["t2"]),
                    int.Parse(data["THROTTLE"]["t3"]),
                    int.Parse(data["THROTTLE"]["t4"]),
                    int.Parse(data["THROTTLE"]["t5"]),
                    int.Parse(data["THROTTLE"]["t6"])
                }
            );


            ThrottleHardLimitChanger = new LimiterChanger(int.Parse(data["SPEED"]["min"]),
                int.Parse(data["SPEED"]["max"])
            );
            throttleLimitChanger = new MultiRangeChanger(new[] {
                new MapRangeChanger(new RangeMapping { minFrom = 0, maxFrom = 127, minTo = ThrottleHardLimitChanger.Min, maxTo = 127 }),
                new MapRangeChanger(new RangeMapping { minFrom = 128, maxFrom = 255, minTo = 128, maxTo = ThrottleHardLimitChanger.Max })
            });

            
            SteerHardLimitChanger = new LimiterChanger(0, 255);

            brakeChanger = new BrakeChanger(126, int.Parse(data["BRAKE"]["cycles"]));

            //------------------------------------------------

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
                //.Chain(new MapRangeChanger(gamepadRangeMapping))
                .Chain(SteerHardLimitChanger)
                ;
            chgInputThrottle = new InputChanger();
            chgThrottle = chgInputThrottle
                //.Chain(new MapRangeChanger(gamepadRangeMapping))
                .Chain(throttleCurveChanger)
                .Chain(throttleLimitChanger)              
                .Chain(ThrottleHardLimitChanger)
                .Chain(brakeChanger)
                ;

        }


        // LOOP -----------------------------------------------------------------

        public void loopInputControls(ref short[] results)
        {
            //always reset values
            resetResults(ref results);
            chgInputThrottle.Input = (int)results[CHthrottle];
            chgInputSteer.Input = (int)results[CHsteer];


            //gPad1 = GamePad.GetState(PlayerIndex.One);
            //chgInputSteer.Input = (int)(gPad1.ThumbSticks.Left.X * ushort.MaxValue);
            //chgInputThrottle.Input = (int)(gPad1.ThumbSticks.Right.Y * ushort.MaxValue);

            //results[CHsteer] = (short)(chgSteer.ComputeValue());
            //results[CHthrottle] = (short)(chgThrottle.ComputeValue());


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
                    setArmed(false);                   
                }
                else
                if (!inCentr)
                {
                    // steer ----------------------------------
                    
                        if (mX <= centrMarginX)
                        {
                            chgInputSteer.Input = (int)(127 - (centrMarginX - mX) * (127.0 / centrMarginX));
                        }
                        else if (mX > centrMarginXEnd) 
                        {
                            chgInputSteer.Input = (int)(127 + (mX - centrMarginXEnd) * (127.0 / centrMarginX));
                        }

                    // throttle ------------------------------------

                        if (mY <= centrMarginY)
                        {
                            chgInputThrottle.Input = (int)(127 + ((centrMarginY - mY) * (127.0 / (centrMarginY))));
                        }
                        else if (mY > centrMarginYEnd)
                        {
                            chgInputThrottle.Input = (int)(127 - ((mY - centrMarginYEnd) * (127.0 / (centrMarginY))));
                        }
                    
                }
            } else
            {
                if (inCentr)
                {
                    setArmed(true);
                }
            }

            chgContext.cycles++;

            results[CHthrottle] = (short)chgThrottle.ComputeValue(chgContext);
            results[CHsteer] = (short)chgSteer.ComputeValue(chgContext);


            //XAxisTextBox.Text = mouseLocation.X.ToString() + ", " + mouseLocation.Y.ToString() + "  : " + b.ToString();
            XAxisTextBox.Text = gPad1.ThumbSticks.Left.X + ", " + gPad1.ThumbSticks.Right.Y
                + "\nst: " + results[CHsteer].ToString() + ", th: " + results[CHthrottle].ToString()
                + "\nc: " + inCentr.ToString() + "  : b: " + inBordr.ToString() + "\narmed: " + armed.ToString() + "\n"
                + "\n[" + mX.ToString() + ", " + mY.ToString() + "]"
                + "\n ml [" + mouseLocationBordr.X.ToString() + ", " + mouseLocationBordr.Y.ToString() + "]"
                + "\n " + chgContext.cycles.ToString()
                ; 
                // + "\ncentrX=" + centrR.x;
                // + "\n" + (centrR.y - mY).ToString() + ":  " + ((double)maxSpeed / (centrR.y - bordrR.y)).ToString();
        }
    }
}
