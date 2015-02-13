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
using System.Linq;

namespace ErrorCode.Domain
{
    public class TestClass
    {
        private readonly IReadOnlyList<Test> _tests;
        private readonly Type _type;

        public TestClass(Type type)
        {
            _type = type;
            _tests = type.GetMethods()
                         .Select(x => new Test(x))
                         .Where(x => x.IsTestable)
                         .ToArray();
        }

        public IReadOnlyList<TestResult> Run(double interval = 100d)
        {
            var instance = Activator.CreateInstance(_type);

            var result = _tests.Select(x => x.Run(instance, interval))
                               .ToArray();

            return result;
        }
    }
}