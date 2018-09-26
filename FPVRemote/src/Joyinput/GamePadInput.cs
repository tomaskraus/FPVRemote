using XInputDotNetPure;

namespace FPVRemote.Joyinput
{
    class GamePadInput : AJoyInput
    {
        private GamePadState state;

        protected override void PollImpl()
        {
            state = GamePad.GetState(PlayerIndex.One);

            vals.x = (int)(state.ThumbSticks.Left.X * ushort.MaxValue);
        }
    }
}
