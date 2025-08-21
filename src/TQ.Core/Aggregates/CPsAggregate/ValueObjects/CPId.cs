using TQ.SharedKernel;

namespace TQ.Core.Aggregates.CPsAggregate.ValueObjects
{
    public class CPId : ValueObject
    {
        private CPId()
        {
            Value = string.Empty;
        }

        public CPId(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public override string ToString() => Value;
    }
}
