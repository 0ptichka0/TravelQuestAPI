namespace TQ.SharedKernel.ValueObjects
{
    public class IntId : ValueObject
    {
        public int Value { get; private set; }
        public IntId(int value)
        {
            Value = value;
        }
        public override string ToString() => Value.ToString();
    }
}
