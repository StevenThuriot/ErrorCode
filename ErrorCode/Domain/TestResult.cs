namespace ErrorCode.Domain
{
    public class TestResult
    {
        public static readonly TestResult Successful = new TestResult(true);
        public static readonly TestResult Faulty = new TestResult(false);

        public TestResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public TestResult(bool succeeded, double average)
            : this(succeeded)
        {
            Average = average;
        }

        public double Average { get; }
        public bool Succeeded { get; }

        public string Message { get; private set; }

        public static TestResult Success(double average, string message = null) => new TestResult(true, average) { Message = message };

        public static TestResult Fault(string message = null) => new TestResult(false) { Message = message };
    }
}