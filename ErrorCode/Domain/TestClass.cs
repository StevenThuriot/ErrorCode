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