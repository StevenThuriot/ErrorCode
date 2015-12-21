namespace ErrorCode.Domain
{
    public class FaultyTestRestult : TestResult
    {
        public FaultyTestRestult()
            : this("Faulty Run")
        {
        }

        public FaultyTestRestult(string message)
            : base(0D, message)
        {
        }

        public override bool Succeeded => false;
    }
}
