using System;

namespace AlertCenter.Exceptions
{
    public class ResourceRequiredException : HttpException
    {
        public ResourceRequiredException(int code = 409) : base("That resource is required by something else", code)
        {
        }

        public ResourceRequiredException(string message, int code = 409) : base(message, code)
        {
        }

        public ResourceRequiredException(string message, Exception innerException, int code = 409) : base(message, innerException, code)
        {
        }
    }
}
