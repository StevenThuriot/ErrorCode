using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ErrorCode.Domain;

namespace ErrorCode
{
    static class Discover
    {
        public static IReadOnlyCollection<TestAssembly> Tests(string assemblyFilter = "*test*.dll")
        {
            var tests = Directory.GetFiles(Directory.GetCurrentDirectory(), assemblyFilter)
                                 .Select(Assembly.LoadFrom)
                                 .Select(x => new TestAssembly(x))
                                 .ToArray();

            return tests;
        }
    }
}