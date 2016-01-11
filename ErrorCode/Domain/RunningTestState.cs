using System;

namespace ErrorCode.Domain
{
    class RunningTestState : TestState
    {
        public RunningTestState()
            : base("Test Running")
        {
        }

        public RunningTestState(string message)
            : base(message)
        {
        }

        public override bool Succeeded => false;
        public override bool Running => true;
        public DateTime StartTime { get; } = DateTime.Now;
    }
}
