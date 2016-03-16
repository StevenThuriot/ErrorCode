﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ErrorCode.Extensions;

namespace ErrorCode.Domain
{
    class TestAssembly : IReadOnlyList<TestClass>
    {
        private readonly IReadOnlyList<TestClass> _tests;

        public string Name { get; }

        public TestAssembly(Assembly assembly)
        {
            Name = assembly.GetName().Name.AsReadable();

            _tests = assembly.GetTypes()
                             .Where(x => x.CustomAttributes.Any(a => a.AttributeType.Name == "TestClassAttribute"))
                             .Select(x => new TestClass(x))
                             .ToArray();
        }

        public IReadOnlyList<TestState> Run(int interval = Constants.DefaultInterval)
        {
            var result = _tests.SelectMany(x => x.Run(interval))
                               .ToArray();

            return result;
        }


        public IEnumerator<TestClass> GetEnumerator() => _tests.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _tests.Count;

        public int TestCount => _tests.SelectMany(x => x).Count();

        public TestClass this[int index] => _tests[index];
    }
}