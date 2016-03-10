using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ErrorCode.Domain;
using System.Text.RegularExpressions;

namespace ErrorCode
{
    static class Discover
    {
        static Regex _assemblyFilter = new Regex(@".*tests?\.(dll|exe)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static IReadOnlyCollection<TestAssembly> Tests() => Tests(_assemblyFilter);

        public static IReadOnlyCollection<TestAssembly> Tests(Regex assemblyFilter)
        {
            var tests = Directory.GetFiles(Directory.GetCurrentDirectory())
                                 .Where(x => assemblyFilter.IsMatch(Path.GetFileName(x)))
                                 .Select(Assembly.LoadFrom)
                                 .Select(x => new TestAssembly(x))
                                 .ToArray();

            return tests;
        }
    }
}