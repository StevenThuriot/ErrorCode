using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ErrorCode.Extensions;
using Horizon;

namespace ErrorCode.Domain
{
    class TestClass : IReadOnlyList<Test>
    {
        readonly IReadOnlyList<Test> _tests;
        readonly Type _type;

        public string Name { get; }


        public TestClass(Type type)
        {
            _type = type;
            Name = type.Name.AsReadable();


            _tests = Info.Extended.Methods(type)
                         .Select(x => new Test(this, x))
                         .Where(x => x.IsTestable)
                         .ToArray();
        }

        public IReadOnlyList<TestState> Run(double interval = Constants.DefaultInterval)
        {
            object instance = CreateTestInstance();

            var result = _tests.Select(x => x.Run(instance, interval))
                               .ToArray();

            return result;
        }

        public object CreateTestInstance() => (object)Info.Create(_type);

        public IEnumerator<Test> GetEnumerator() => _tests.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _tests.Count;

        public Test this[int index] => _tests[index];
    }
}