using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Horizon;
using ErrorCode.Base;
using System.Text;

namespace ErrorCode.Domain
{
    class Test : Notifyable
    {
        readonly Attribute _expException;
        readonly IMethodCaller _caller;

        TestResult _testResult;
        public TestResult TestResult
        {
            get { return _testResult; }
            set { ChangeProperty(ref _testResult, value); }
        }

        public TestClass Parent { get; }

        public Test(TestClass parent, IMethodCaller caller)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            if (caller == null)
                throw new ArgumentNullException(nameof(caller));
            
            _caller = caller;
            Parent = parent;

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
        
        public string ReadableName => AsReadable(Name);

        static string AsReadable(string text)
        {
            var builder = new StringBuilder();

            var typeName = text;
            int index = -1;

            while (++index < typeName.Length)
            {
                char nextChar = typeName[index];

                if (char.IsUpper(nextChar) && index != 0)
                {
                    if (char.IsLower(typeName[index - 1]))
                    {
                        if (typeName.Length > index + 1 &&
                            char.IsLower(typeName[index + 1]))
                        {
                            nextChar = char.ToLower(nextChar);
                        }
                        builder.Append(' ');
                    }
                    else if (typeName.Length > index + 1 && char.IsLower(typeName[index + 1]))
                    {
                        builder.Append(' ');
                        nextChar = char.ToLower(nextChar);
                    }
                }

                builder.Append(nextChar);
            }

            return builder.ToString();
        }

        public TestResult Run(dynamic testClass, double interval = Constants.DefaultInterval) => TestResult = RunInternal(testClass, interval);

        TestResult RunInternal(dynamic testClass, double interval)
        {
            if (!IsTestable)
                return TestResult.Fault("Untestable.");

            try
            {
                if (_expException != null)
                {
                    return RunTestWithExpectedException(testClass, _expException);
                }
                else
                {
                    double totalMilliseconds = RunTest(testClass, interval);
                    var average = totalMilliseconds / interval;

                    return TestResult.Success(average);
                }
            }
            catch (Exception ex)
            {
                //Fail 
                return TestResult.Fault("Unexpected error: " + ex.Message);
            }
        }

        double RunTest(dynamic test, double interval)
        {
            var parameters = new dynamic[] { test };
            //Warmup
            _caller.Call(parameters);

            // Clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            //Testing
            var watch = Stopwatch.StartNew();

            for (var i = 0; i < interval; i++)
                _caller.Call(parameters);

            watch.Stop();

            return watch.Elapsed.TotalMilliseconds;
        }

        TestResult RunTestWithExpectedException(dynamic test, Attribute expException)
        {
            try
            {
                _caller.Call(new dynamic[] { test });
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

            return TestResult.Success();
        }
    }
}