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

        public int yOffset;

        bool armed;

        short minSpeed;
        short maxSpeed;

        int deadZoneX;
        int deadZoneY;

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

        public void initInputControls(IniData data, ref short[] initialResults)
        {
            for (int i = 0; i < initialResults.Length; i++)
            {
                initialResults[i] = 128;
            }

            //------------------------------------------------

            minSpeed = short.Parse(data["SPEED"]["min"]);
            maxSpeed = short.Parse(data["SPEED"]["max"]);

            yOffset = short.Parse(data["OFFSET"]["y"]);

            centrR = new MyRect(int.Parse(data["CENTER"]["x"]), int.Parse(data["CENTER"]["y"]), int.Parse(data["CENTER"]["w"]), int.Parse(data["CENTER"]["h"]));
            bordrR = new MyRect(int.Parse(data["BORDER"]["x"]), int.Parse(data["BORDER"]["y"]), int.Parse(data["BORDER"]["w"]), int.Parse(data["BORDER"]["h"]));

            deadZoneX = int.Parse(data["DEADZONE"]["x"]);
            deadZoneY = int.Parse(data["DEADZONE"]["y"]);

            armed = false;

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
            //Point mouseLocationP = System.Windows.Input.Mouse.GetPosition(CameraCanvas);
            Point mouseLocationBordr = bordr.PointFromScreen(mouseLocation);
            Point mouseLocationCentr = centr.PointFromScreen(mouseLocation);
            int mX = (int)mouseLocation.X;
            int mY = (int)mouseLocation.Y - yOffset;

            bool inBordr = bordr.RenderedGeometry.FillContains(mouseLocationBordr);
            bool inCentr = centr.RenderedGeometry.FillContains(mouseLocationCentr);


            if (armed)
            {
                if (!inBordr)
                {
                    results[CHsteer] = 127;
                    results[CHthrottle] = 127;
                    armed = false;
                }
                else
                if (inCentr)
                {
                    // steer ----------------------------------
                    
                        if (mX <= centrR.x)
                        {
                            results[CHsteer] = (short)(127 - (centrR.x - mX) * (127.0 / (centrR.x - bordrR.x - deadZoneX)));
                        }
                        else if (mX > centrR.xMax) 
                        {
                            results[CHsteer] = (short)(127 + (mX - centrR.xMax) * (127.0 / (centrR.x - bordrR.x - deadZoneX)));
                        }

                    // gas ------------------------------------

                        if (mY <= centrR.y)
                        {
                            results[CHthrottle] = (short)(127 + ((centrR.y - mY) * ((double)(maxSpeed - 127) / (centrR.y - bordrR.y - deadZoneY))));
                        }
                        else if (mY > centrR.yMax)
                        {
                            results[CHthrottle] = (short)(127 - ((mY - centrR.yMax) * ((double)(127 - minSpeed) / (bordrR.h - centrR.h - (centrR.y - bordrR.y - deadZoneY)))));
                        }
                    
                }
            } else
            {
                if (inCentr)
                {
                    armed = true;
                }
            }

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


            


            //XAxisTextBox.Text = mouseLocation.X.ToString() + ", " + mouseLocation.Y.ToString() + "  : " + b.ToString();
            XAxisTextBox.Text = gPad1.ThumbSticks.Left.X + ", " + gPad1.ThumbSticks.Right.Y
                + "\n" + results[CHsteer].ToString() + ", " + results[CHthrottle].ToString()
                + "\nc: " + inCentr.ToString() + "  : b: " + inBordr.ToString() + "\narmed: " + armed.ToString() + "\n"
                + "\n[" + mX.ToString() + ", " + mY.ToString() + "]"
                + "\n ml [" + mouseLocationBordr.X.ToString() + ", " + mouseLocationBordr.Y.ToString() + "]"
                ; 
                // + "\ncentrX=" + centrR.x;
                // + "\n" + (centrR.y - mY).ToString() + ":  " + ((double)maxSpeed / (centrR.y - bordrR.y)).ToString();
        }
    }
}
