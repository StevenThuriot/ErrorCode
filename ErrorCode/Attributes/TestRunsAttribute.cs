using System;

namespace ErrorCode
{
    public class TestRunsAttribute : Attribute
    {
        public readonly uint Runs;

        public TestRunsAttribute(uint runs)
        {
            Runs = runs;
        }
    }
}