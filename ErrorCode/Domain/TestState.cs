using System;

namespace ErrorCode.Domain
{
    public abstract class TestState
    {
        public TestState(string message)
        {
            Message = message;
        }

        public abstract bool Succeeded { get; }
        public abstract bool Running { get; }
        public string Message { get; }

        public static TestState Success(double average = 0D, string message = null) => new SuccessfulTestState(average, message);

        public static TestState Fault(string message = null) => new FaultyTestState(message);

        public override string ToString() => $"Test run successful: {Succeeded}{Environment.NewLine}• Message: {Message}";
    }
}