using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Horizon;

namespace ErrorCode.Domain
{
    class Test
    {
        private readonly Attribute _expException;
        private IMethodCaller _caller;

        public TestResult TestResult { get; private set; }

        public Test(IMethodCaller caller)
        {
            _caller = caller;
            
            var comparer = StringComparer.OrdinalIgnoreCase;

            CustomAttributes = Attribute.GetCustomAttributes(_caller.MethodInfo);
            IsTestable = CustomAttributes.Any(a => comparer.Equals(a.GetType().Name, "TestMethodAttribute"));

            if (!IsTestable)
                return;//Don't bother if it's not testable.
            
            _expException = CustomAttributes.FirstOrDefault(x => comparer.Equals(x.GetType().Name, "ExpectedExceptionAttribute"));
        }

        public IReadOnlyList<Attribute> CustomAttributes { get; }
        public bool IsTestable { get; }
        public string Name => _caller.Name;

        public TestResult Run(dynamic testClass, double interval = Constants.DefaultInterval) => TestResult = RunInternal(testClass, interval);

        private TestResult RunInternal(dynamic testClass, double interval)
        {
            if (!IsTestable)
                return TestResult.Fault("Untestable.");

            try
            {
                if (_expException != null)
                {
                    bool success = RunTestWithExpectedException(testClass, _expException);
                    return new TestResult(success);
                }
                double totalMilliseconds = RunTest(testClass, interval);
                var average = totalMilliseconds / interval;

                return TestResult.Success(average);
            }
            catch (Exception ex)
            {
                //Fail 
                return TestResult.Fault("Unexpected error: " + ex.Message);
            }
        }

        private double RunTest(dynamic test, double interval)
        {
            //Warmup
            _caller.Call(test);

            // Clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            //Testing
            var watch = Stopwatch.StartNew();

            for (var i = 0; i < interval; i++)
                _caller.Call(test);

            watch.Stop();

            return watch.Elapsed.TotalMilliseconds;
        }

        private TestResult RunTestWithExpectedException(dynamic test, Attribute expException)
        {
            try
            {
                _caller.Call(test);
                return TestResult.Fault("Test did not throw expected Type.");
            }
            catch (Exception ex)
            {
                dynamic expectedException = expException;
                Type exceptionType = expectedException.ExceptionType;

                if (ex.GetType() != exceptionType)
                {
                    return TestResult.Fault("Test did not throw expected Type.");
                }

                string message = expectedException.ExpectedMessage;

                if (!string.IsNullOrEmpty(message) && ex.Message != message)
                {
                    return TestResult.Fault("Test threw expected Type, but with an unexpected message.");
                }
            }

            return TestResult.Successful;
        }
    }
}