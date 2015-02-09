using System;
using System.Collections.Generic;
using ErrorCode.Base;

namespace ErrorCode.ViewModels
{
	public class Overview : Notifyable
	{
		public Overview()
		{
			Tests = Discover.Tests();
		}

		public IEnumerable<Type> Tests { get; private set; }
	}
}
