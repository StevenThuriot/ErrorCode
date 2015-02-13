#region License

//  
// Copyright 2015 Steven Thuriot
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Invocation;

namespace ErrorCode.Domain
{
    public class Test
    {
        private readonly Attribute _expException;
        private readonly Lazy<Delegate> _method;

        public Test(MethodInfo methodInfo)
        {
            Name = methodInfo.Name.Skip(1)
                             .Aggregate(methodInfo.Name[0].ToString(CultureInfo.InvariantCulture),
                                        (current, next) => current + (char.IsUpper(next) ? " " : "") + next);

            var comparer = StringComparer.OrdinalIgnoreCase;

            CustomAttributes = Attribute.GetCustomAttributes(methodInfo);
            IsTestable = CustomAttributes.Any(a => comparer.Equals(a.GetType().Name, "TestMethodAttribute"));

            if (!IsTestable) return;

            //Don't bother if it's not testable.
            _method = methodInfo.BuildLazy();
            _expException =
                CustomAttributes.FirstOrDefault(x => comparer.Equals(x.GetType().Name, "ExpectedExceptionAttribute"));
        }

        public IReadOnlyList<Attribute> CustomAttributes { get; private set; }
        public bool IsTestable { get; private set; }
        public string Name { get; private set; }

        public TestResult Run(dynamic testClass, double interval)
        {
            if (!IsTestable) return TestResult.Fault("Untestable.");
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