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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ErrorCode.Extensions;

namespace ErrorCode.Domain
{
    public class TestClass : IReadOnlyList<Test>
    {
        private readonly IReadOnlyList<Test> _tests;
        private readonly Type _type;

        public string Name { get; private set; }


        public TestClass(Type type)
        {
            _type = type;
            Name = type.Name.AsReadable();

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

        public IEnumerator<Test> GetEnumerator()
        {
            return _tests.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get { return _tests.Count; } }

        public Test this[int index]
        {
            get { return _tests[index]; }
        }
    }
}