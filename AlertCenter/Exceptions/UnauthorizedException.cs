﻿using System;

namespace AlertCenter.Exceptions
{
    public class UnauthorizedException : HttpException
    {
        public UnauthorizedException(int code = 401) : base("Not authorized", code)
        {
        }

        public UnauthorizedException(string message, int code = 401) : base(message, code)
        {
        }

        public UnauthorizedException(string message, Exception innerException, int code) : base(message, innerException, code)
        {
        }
    }
}
