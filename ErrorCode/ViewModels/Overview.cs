using System.Collections.Generic;
using ErrorCode.Base;
using ErrorCode.Domain;

namespace ErrorCode.ViewModels
{
    class Overview : ViewModel<Overview>
    {
        public Overview()
        {
            Tests = Discover.Tests();
        }

        public IEnumerable<TestAssembly> Tests { get; }
    }
}