namespace FPVRemote.Joyinput
{
    /// <summary>
    /// (like a trait)
    /// </summary>
    public abstract class AJoyInput : IJoyInput
    {
        protected JoyValues vals;

        public JoyValues Values { get => vals; }
        public IJoyInput SrcInput { get; set; }

        public IJoyInput Chain(IJoyInput nextInput) {
            nextInput.SrcInput = this;
            return nextInput;
        }

        public void Poll()
        {
            if (SrcInput != null)
            {
                SrcInput.Poll();
                vals = SrcInput.Values;
            }
            PollImpl();
        }

        protected abstract void PollImpl();
    }
}
