﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Horizon;
using ErrorCode.Base;
using System.Text;

namespace ErrorCode.Domain
{
    class Test : SelectableItem
    {
        readonly Attribute _expException;
        readonly IMethodCaller _caller;

        TestState _testState = NotRunTestState.Instance;
        public TestState TestState
        {
            get { return _testState; }
            set { SetValue(ref _testState, value); }
        }
        

        public Test(TestClass parent, IMethodCaller caller)
        {
            if (parent == null)
                throw new NullReferenceException(nameof(parent));

            if (caller == null)
                throw new ArgumentNullException(nameof(caller));

            Parent = parent;
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
        
        public string ReadableName => AsReadable(Name);

        public TestClass Parent { get; }

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

        public void Run(dynamic testClass, int interval = Constants.DefaultInterval)
        {
            var expanded = IsExpanded;
            var selected = IsSelected;

            IsExpanded = true;
            IsSelected = true;


            if (!IsTestable)
            {
                TestState = TestState.Fault("Untestable.");
                return;
            }

            try
            {
                TestState = new RunningTestState(interval);

#if DEBUG
                //System.Threading.Thread.Sleep(2500);
#endif

                if (_expException != null)
                {
                    TestState = RunTestWithExpectedException(testClass, _expException);
                }
                else
                {
                    double totalMilliseconds = RunTest(testClass, interval);
                    var average = totalMilliseconds / interval;

                    TestState = TestState.Success(average, interval);
                }
            }
            catch (Exception ex)
            {
                //Fail 
                TestState = TestState.Fault("Unexpected error: " + ex.Message);
            }
            finally 
            {
                if (TestState is RunningTestState)
                {
                    TestState = null;
                }
                
                IsExpanded = expanded;
                IsSelected = selected;

                Parent.NotifyChanges();
            }
        }

        double RunTest(dynamic test, int interval)
        {
            var parameters = new[] { test };
            //Warmup
            _caller.Call(parameters);

            // Clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            //Testing
            var watch = new Stopwatch();

            for (var i = 0; i < interval; i++)
            {
                watch.Start();
                _caller.Call(parameters);
                watch.Stop();
            }

            return watch.Elapsed.TotalMilliseconds;
        }

        TestState RunTestWithExpectedException(dynamic test, Attribute expException)
        {
            try
            {
                _caller.Call(new [] { test });
                return TestState.Fault("Test did not throw expected Type.");
            }
            catch (Exception ex)
            {
                dynamic expectedException = expException;
                Type exceptionType = expectedException.ExceptionType;

                if (ex.GetType() != exceptionType)
                {
                    return TestState.Fault("Test did not throw expected Type.");
                }

                try
                {
                    var message = (string)Horizon.Info.Call(expectedException, "GetNoExceptionMessage");
                    if (!string.IsNullOrEmpty(message) && ex.Message != message)
                    {
                        return TestState.Fault("Test threw expected Type, but with an unexpected message.");
                    }
                }
                catch
                {
                    //ignore if something goes wrong;
                }
            }

            return TestState.Success();
        }
    }
}