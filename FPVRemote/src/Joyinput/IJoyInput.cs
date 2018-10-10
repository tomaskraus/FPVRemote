using System;

namespace FPVRemote.Joyinput
{

    public interface IJoyInput
    {

        IJoyInput SrcInput
        {
            get; set;
        }

        /// <summary>
        /// Computes joystick's value.
        /// </summary>
        int ComputeValue();

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
