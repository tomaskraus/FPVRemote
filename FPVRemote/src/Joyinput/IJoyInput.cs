using System;

namespace FPVRemote.Joyinput
{
    /// <summary>
    /// holds joystick's controls values
    /// </summary>
    public struct JoyValues
    {
        public int x;
        public int y;

        public override string ToString()
        {
            return x + ", " + y;
        }
    }

    public interface IJoyInput
    {

        JoyValues Values
        {
            get;
        }

        IJoyInput SrcInput
        {
            get; set;
        }

        /// <summary>
        /// Polls joystick's state of the controls.
        /// </summary>
        void Poll();

        /// <summary>
        /// Chains iJoyInput easily. Assigns this as nextInput's srcInput.
        /// </summary>
        /// <example>
        ///     BaseJoy
        ///       .chain(LimitJoy)
        ///       .chain(ReverseJoy)       
        /// </example>
        /// <param name="nextInput"></param>
        /// <returns>nextInput</returns>
        IJoyInput Chain(IJoyInput nextInput);

    }
}
