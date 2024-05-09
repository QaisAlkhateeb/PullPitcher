using System;

namespace PullPitcher.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message)
            : base(message) { }
    }
}
