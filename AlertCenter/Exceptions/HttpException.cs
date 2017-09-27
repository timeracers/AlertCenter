using System;

namespace AlertCenter.Exceptions
{
    public class HttpException : Exception
    {
        public int StatusCode { get; set; }

        public HttpException(int code) : base()
        {
            StatusCode = code;
        }

        public HttpException(string message, int code) : base(message)
        {
            StatusCode = code;
        }

        public HttpException(string message, Exception innerException, int code) : base(message, innerException)
        {
            StatusCode = code;
        }

        public override string ToString()
        {
            return InnerException == null ? StatusCode + ": " + GetType().Name + ": " + Message :
                StatusCode + ": " + GetType() + ": " + Message + " InnerException: " + InnerException.GetType() + ": " + InnerException.Message;
        }
    }
}