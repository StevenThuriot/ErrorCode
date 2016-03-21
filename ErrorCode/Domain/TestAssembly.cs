using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ErrorCode.Base;
using ErrorCode.Extensions;

namespace ErrorCode.Domain
{
    class TestAssembly : SelectableItem, IReadOnlyList<TestClass>
    {
        readonly IReadOnlyList<TestClass> _tests;

        public string Name { get; }

        public TestAssembly(Assembly assembly)
        {
            Name = assembly.GetName().Name.AsReadable();

            _tests = assembly.GetTypes()
                             .Where(x => x.CustomAttributes.Any(a => a.AttributeType.Name == "TestClassAttribute"))
                             .Select(x => new TestClass(this, x))
                             .ToArray();
        }

        public IReadOnlyList<TestState> Run(int interval = Constants.DefaultInterval)
        {
            var expanded = IsExpanded;
            var selected = IsSelected;

            IsExpanded = true;
            IsSelected = true;

            var result = _tests.SelectMany(x => x.Run(interval))
                               .ToArray();

            IsExpanded = expanded;
            IsSelected = selected;


            return result;
        }


        public IEnumerator<TestClass> GetEnumerator() => _tests.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _tests.Count;

        public int TestCount => _tests.SelectMany(x => x).Count();

        public TestClass this[int index] => _tests[index];
        
        public int TestsRun => _tests.Sum(x => x.TestsRun);
        public int TestsSucceeded => _tests.Sum(x => x.TestsSucceeded);
        public int TestsFailed => _tests.Sum(x => x.TestsFailed);

        public void NotifyChanges()
        {
            OnPropertiesChanged(nameof(TestsRun), nameof(TestsSucceeded), nameof(TestsFailed));
        }
    }
}