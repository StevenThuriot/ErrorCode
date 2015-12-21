using System;

namespace ErrorCode.Domain
{
    public abstract class TestResult
    {
        public TestResult(double average, string message)
        {
            Average = average;
            Message = message;
        }

        public abstract bool Succeeded { get; }

        public double Average { get; }
        public string Message { get; }

        public static TestResult Success(double average = 0D, string message = null) => new SuccessfulTestRestult(average, message);

        public static TestResult Fault(string message = null) => new FaultyTestRestult(message);

        public override string ToString() => $"Test run successful: {Succeeded}{Environment.NewLine}• Message: {Message}";
    }
}