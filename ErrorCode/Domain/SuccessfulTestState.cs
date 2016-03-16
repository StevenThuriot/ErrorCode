namespace ErrorCode.Domain
{
    public class SuccessfulTestState : TestState
    {
        public SuccessfulTestState()
            : base("Successful Run")
        {
            Average = 0D;
        }

        public SuccessfulTestState(double average, int interval, string message)
            : base(message)
        {
            Average = average;
            Interval = interval;
        }

        public override bool Succeeded => true;
        public override bool Running => false;

        public double Average { get; }
        public int Interval { get; }
    }
}
