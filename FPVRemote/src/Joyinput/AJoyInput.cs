namespace FPVRemote.Joyinput
{
    /// <summary>
    /// (like a trait)
    /// </summary>
    public abstract class AJoyInput : IJoyInput
    {
        protected int value;

        public IJoyInput SrcInput { get; set; }

        public IJoyInput Chain(IJoyInput nextInput) {
            nextInput.SrcInput = this;
            return nextInput;
        }

        public int ComputeValue()
        {
            if (SrcInput != null)
            {
                value = SrcInput.ComputeValue();                                                                                            
            }
            return ComputeImpl(value);
        }

        protected abstract int ComputeImpl(int val);
    }
}
