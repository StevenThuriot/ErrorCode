﻿#region License

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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ErrorCode.Extensions;

namespace ErrorCode.Domain
{
    public class TestAssembly : IReadOnlyList<TestClass>
    {
        private readonly IReadOnlyList<TestClass> _tests;

        public string Name { get; private set; }

        public TestAssembly(Assembly assembly)
        {
            Name = assembly.GetName().Name.AsReadable();

            _tests = assembly.GetTypes()
                             .Where(x => x.CustomAttributes.Any(a => a.AttributeType.Name == "TestClassAttribute"))
                             .Select(x => new TestClass(x))
                             .ToArray();
        }

        public IReadOnlyList<TestResult> Run(double interval = 100d)
        {
            var result = _tests.SelectMany(x => x.Run(interval))
                               .ToArray();

            return result;
        }


        public IEnumerator<TestClass> GetEnumerator()
        {
            return _tests.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get { return _tests.Count; } }

        public TestClass this[int index]
        {
            get { return _tests[index]; }
        }
    }
}