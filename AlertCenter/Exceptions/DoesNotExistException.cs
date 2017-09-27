using System;

namespace AlertCenter.Exceptions
{
    public class DoesNotExistException : HttpException
    {
        public DoesNotExistException(int code = 404) : base("Resource doesn't exist", code)
        {
        }

        public DoesNotExistException(string message, int code = 404) : base(message, code)
        {
        }

        public DoesNotExistException(string message, Exception innerException, int code = 404) : base(message, innerException, code)
        {
        }
    }
}
