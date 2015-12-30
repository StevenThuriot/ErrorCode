namespace ErrorCode.Domain
{
    public class FaultyTestState : TestState
    {
        public FaultyTestState()
            : base("Faulty Run")
        {
        }

        public FaultyTestState(string message)
            : base(message)
        {
        }

        public override bool Succeeded => false;
    }
}
