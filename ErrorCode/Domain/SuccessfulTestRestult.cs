namespace ErrorCode.Domain
{
    public class SuccessfulTestRestult : TestResult
    {
        public SuccessfulTestRestult()
            : this(0D, "Successful Run")
        {
        }

        public SuccessfulTestRestult(double average, string message)
            : base(average, message)
        {
        }

        public override bool Succeeded => true;
    }
}
