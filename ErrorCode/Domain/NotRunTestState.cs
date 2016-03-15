namespace ErrorCode.Domain
{
    public class NotRunTestState : TestState
    {
        public static readonly TestState Instance = new NotRunTestState();

        NotRunTestState() : base("Test Yet To Run")
        {
        }

        public override bool Succeeded => false;
        public override bool Running => false;
    }
}
