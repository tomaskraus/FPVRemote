using XInputDotNetPure;

namespace FPVRemote.Joyinput
{
    class GamePadInput : AJoyInput
    {
        private GamePadState state;

        protected override int ComputeImpl(int val)
        {
            state = GamePad.GetState(PlayerIndex.One);

            return (int)(state.ThumbSticks.Left.X * ushort.MaxValue);
        }
    }
}
