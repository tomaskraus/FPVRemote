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

        public int ComputeValue(ChangerContext cc)
        {
            if (SrcChanger != null)
            {
                value = SrcChanger.ComputeValue(cc);                                                                                            
            }
            return ComputeImpl(value, cc);
        }

        public int ComputeValueDirectly(int val, ChangerContext cc)
        {
            return ComputeImpl(val, cc);
        }

        protected abstract int ComputeImpl(int val, ChangerContext cc);
    }
}
