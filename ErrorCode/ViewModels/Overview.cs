using System.Collections.Generic;
using ErrorCode.Base;
using ErrorCode.Domain;

namespace ErrorCode.ViewModels
{
    public class Overview : ViewModel<Overview>
    {
        public Overview()
        {
            Tests = Discover.Tests();
        }

        public IEnumerable<TestAssembly> Tests { get; private set; }
    }
}