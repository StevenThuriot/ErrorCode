using System;

namespace ErrorCode
{
    public class ExecutionTimeAttribute : Attribute
    {
        public readonly TimeSpan ExecutionTime;

        public ExecutionTimeAttribute(double executionTime)
        {
            ExecutionTime = TimeSpan.FromMilliseconds(executionTime);
        }
    }
}