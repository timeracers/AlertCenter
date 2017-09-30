using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AlertCenter.Middleware
{
    public class ResponseWrapper
    {
        public int StatusCode { get; set; }
        public object Response { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }

        public ResponseWrapper(int statusCode)
        {
            StatusCode = statusCode;
        }
    }

    //Used for swagger
    public class ResponseWrapper<T>
    {
        public int StatusCode { get; set; }
        public T Response { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
    }

    public class None { }
}
