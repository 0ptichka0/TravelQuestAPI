using TQ.SharedKernel;

namespace TQ.Core.Aggregates.RunsAggregate.ValueObjects
{
    public class RunId : ValueObject
    {
        private RunId()
        {
            Value = string.Empty;
        }

        public RunId(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public override string ToString() => Value;
    }
}
