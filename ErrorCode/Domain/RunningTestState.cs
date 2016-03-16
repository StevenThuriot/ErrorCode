using System;

namespace ErrorCode.Domain
{
    class RunningTestState : TestState
    {
        public RunningTestState()
            : this(1)
        {
        }

        public RunningTestState(int interval)
           : this("Test Running", interval)
        {
        }

        public RunningTestState(string message, int interval)
            : base(message)
        {
            Interval = interval;
        }

        public override bool Succeeded => false;
        public override bool Running => true;
        public DateTime StartTime { get; } = DateTime.Now;
        public int Interval { get; }
    }
}
