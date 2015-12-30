namespace ErrorCode.Domain
{
    public class SuccessfulTestState : TestState
    {
        public SuccessfulTestState()
            : base("Successful Run")
        {
            Average = 0D;
        }

        public SuccessfulTestState(double average, string message)
            : base(message)
        {
            Average = average;
        }

        public override bool Succeeded => true;
        public double Average { get; }
    }
}
