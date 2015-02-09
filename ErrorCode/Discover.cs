using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ErrorCode
{
	static class Discover
	{
		public static IReadOnlyCollection<Type> Tests(string assemblyFilter = "*test*.dll")
		{
			var tests = Directory.GetFiles(Directory.GetCurrentDirectory(), assemblyFilter)
								 .Select(Assembly.LoadFrom)
								 .SelectMany(assembly => assembly.GetTypes().Where(x => x.CustomAttributes.Any(a => a.GetType().Name == "TestClassAttribute")));

			return tests.ToArray();
		}
	}
}
