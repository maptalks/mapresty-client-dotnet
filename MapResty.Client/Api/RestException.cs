using System;

namespace MapResty.Client.Api
{
    public class RestException : Exception
    {
        public int ErrorCode { get; private set; }

        public RestException(string message)
            : base(message)
        {
        }

        public RestException(int errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }
    }
}
