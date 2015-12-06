using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ErrorCode.Extensions;
using Horizon;

namespace ErrorCode.Domain
{
    public class Test
    {
        private readonly Attribute _expException;
        private readonly Lazy<Delegate> _method;

        public Test(MethodInfo methodInfo)
        {
            Name = methodInfo.Name.AsReadable();

            var comparer = StringComparer.OrdinalIgnoreCase;

            CustomAttributes = Attribute.GetCustomAttributes(methodInfo);
            IsTestable = CustomAttributes.Any(a => comparer.Equals(a.GetType().Name, "TestMethodAttribute"));

            if (!IsTestable)
                return;

            //Don't bother if it's not testable.
            _method = methodInfo.BuildLazy();

            _expException = CustomAttributes.FirstOrDefault(x => comparer.Equals(x.GetType().Name, "ExpectedExceptionAttribute"));
        }

        public IReadOnlyList<Attribute> CustomAttributes { get; private set; }
        public bool IsTestable { get; private set; }
        public string Name { get; private set; }

        public TestResult Run(dynamic testClass, double interval)
        {
            if (!IsTestable)
                return TestResult.Fault("Untestable.");

            try
            {
                var @delegate = _method.Value;

                if (_expException != null)
                {
                    bool success = RunTestWithExpectedException(@delegate, testClass, _expException);
                    return new TestResult(success);
                }
                double totalMilliseconds = RunTest(@delegate, testClass, interval);
                var average = totalMilliseconds/interval;

                return TestResult.Success(average);
            }
            catch (Exception ex)
            {
                //Fail 
                return TestResult.Fault("Unexpected error: " + ex.Message);
            }
        }


        private static double RunTest(dynamic @delegate, dynamic test, double interval)
        {
            //Warmup
            @delegate(test);

            // Clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            //Testing
            var watch = Stopwatch.StartNew();

            for (var i = 0; i < interval; i++)
                @delegate(test);

            watch.Stop();

            return watch.Elapsed.TotalMilliseconds;
        }

        private static TestResult RunTestWithExpectedException(dynamic @delegate, dynamic test, Attribute expException)
        {
            try
            {
                @delegate(test);
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