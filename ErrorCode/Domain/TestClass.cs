using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ErrorCode.Base;
using ErrorCode.Extensions;
using Horizon;
using System.ComponentModel;

namespace ErrorCode.Domain
{
    class TestClass : SelectableItem, IReadOnlyList<Test>
    {
        readonly IReadOnlyList<Test> _tests;
        readonly Type _type;

        public string Name { get; }


        public TestClass(TestAssembly parent, Type type)
        {
            if (parent == null)
                throw new NullReferenceException(nameof(parent));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Parent = parent;

            _type = type;
            Name = type.Name.AsReadable();


            _tests = Info.Extended.Methods(type)
                         .Select(x => new Test(this, x))
                         .Where(x => x.IsTestable)
                         .ToArray();
        }

        public IReadOnlyList<TestState> Run(int interval = Constants.DefaultInterval)
        {
            var expanded = IsExpanded;
            var selected = IsSelected;

            IsExpanded = true;
            IsSelected = true;

            object instance = CreateTestInstance();

            var result = new TestState[_tests.Count];

            for (int i = 0; i < _tests.Count; i++)
            {
                var test = _tests[i];
                test.Run(instance, interval);

                result[i] = test.TestState;
            }

            IsExpanded = expanded;
            IsSelected = selected;

            return result;
        }

        public object CreateTestInstance() => (object)Info.Create(_type);

        public IEnumerator<Test> GetEnumerator() => _tests.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _tests.Count;

        public TestAssembly Parent { get; }

        public Test this[int index] => _tests[index];

        public int TestsRun => _tests.Count(x => x.TestState != NotRunTestState.Instance);
        public int TestsSucceeded => _tests.Where(x => x.TestState != NotRunTestState.Instance && !x.TestState.Running).Count(x => x.TestState.Succeeded);
        public int TestsFailed    => _tests.Where(x => x.TestState != NotRunTestState.Instance && !x.TestState.Running).Count(x => !x.TestState.Succeeded);

        public void NotifyChanges()
        {
            OnPropertiesChanged(nameof(TestsRun), nameof(TestsSucceeded), nameof(TestsFailed));
        }
    }
}