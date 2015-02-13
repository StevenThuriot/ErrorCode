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

        public double Average { get; private set; }
        public bool Succeeded { get; private set; }

        public string Message { get; private set; }

        public static TestResult Success(double average, string message = null)
        {
            return new TestResult(true, average)
                   {
                       Message = message
                   };
        }

        public static TestResult Fault(string message = null)
        {
            return new TestResult(false)
                   {
                       Message = message
                   };
        }
    }
}