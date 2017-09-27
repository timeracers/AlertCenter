using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlertCenter.Exceptions
{
    public class AlreadyExistsException : HttpException
    {
        public AlreadyExistsException(int code = 409) : base("Resource already exists", code)
        {
        }

        public AlreadyExistsException(string message, int code = 409) : base(message, code)
        {
        }

        public AlreadyExistsException(string message, Exception innerException, int code = 409) : base(message, innerException, code)
        {
        }
    }
}
