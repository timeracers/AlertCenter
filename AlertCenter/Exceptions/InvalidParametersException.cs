using System;

namespace AlertCenter.Exceptions
{
    public class InvalidParametersException : HttpException
    {
        public InvalidParametersException(int code = 400) : base("Invalid parameters", code)
        {
        }

        public InvalidParametersException(string message, int code = 400) : base(message, code)
        {
        }

        public InvalidParametersException(string message, Exception innerException, int code = 400) : base(message, innerException, code)
        {
        }
    }
}