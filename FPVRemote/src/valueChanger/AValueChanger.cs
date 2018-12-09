namespace FPVRemote.valueChanger
{
    /// <summary>
    /// (like a trait)
    /// </summary>
    public abstract class AValueChanger : IValueChanger
    {
        protected int value;

        public IValueChanger SrcChanger { get; set; }

        public IValueChanger Chain(IValueChanger nextChanger) {
            nextChanger.SrcChanger = this;
            return nextChanger;
        }

        public int ComputeValue()
        {
            if (SrcChanger != null)
            {
                value = SrcChanger.ComputeValue();                                                                                            
            }
            return ComputeImpl(value);
        }

        protected abstract int ComputeImpl(int val);
    }
}
