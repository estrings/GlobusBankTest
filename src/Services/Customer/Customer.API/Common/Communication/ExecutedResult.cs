using Customer.API.Data.Enums;
using System.Collections.Generic;

namespace Customer.API.Common.Communication
{
    public class ExecutedResult
    {
        public ExecutedResult()
        {
            Response = ResponseCode.Ok;
        }
        public ResponseCode Response { get; set; }
        public string Message { get; set; }
        public int RetryCount { get; set; }
        public List<string> ValidationMessages { get; set; } = new List<string>();

        public static ExecutedResult Failed(string message)
        {
            return new ExecutedResult { Message = message, Response = ResponseCode.ProcessingError };
        }


        public static ExecutedResult Success(string message)
        {
            return new ExecutedResult { Message = message, Response = ResponseCode.Ok };
        }

    }

    public class ExecutedResult<T> : ExecutedResult
    {
        public T Result { get; set; }
        public static ExecutedResult<T> Success(T result, string message)
        {
            return new ExecutedResult<T> { Response = ResponseCode.Ok, Result = result, Message = message };
        }

        public static new ExecutedResult<T> Failed(string errorMsg)
        {
            return new ExecutedResult<T> { Message = errorMsg, Response = ResponseCode.ProcessingError };
        }

        public static ExecutedResult<T> Failed(T result, string errorMsg)
        {
            return new ExecutedResult<T> { Message = errorMsg, Result = result, Response = ResponseCode.AuthorizationError };
        }


        public static ExecutedResult<T> NotFound(string message)
        {
            return new ExecutedResult<T> { Message = message, Response = ResponseCode.NotFound };
        }

        public static ExecutedResult<T> NotCompleted(string message)
        {
            return new ExecutedResult<T> { Message = message, Response = ResponseCode.NotCompleted };
        }

        public static ExecutedResult<T> NotCompleted(T result, string message)
        {
            return new ExecutedResult<T> { Response = ResponseCode.NotCompleted, Result = result, Message = message };
        }

        public static ExecutedResult<T> AccessDenied(string errorMsg)
        {
            return new ExecutedResult<T> { Message = errorMsg, Response = ResponseCode.AuthorizationError };
        }

        public static ExecutedResult<T> BadRequest(string errorMsg)
        {
            return new ExecutedResult<T> { Message = errorMsg, Response = ResponseCode.ProcessingError };
        }

        public static ExecutedResult<T> Exception(string errorMsg)
        {
            return new ExecutedResult<T> { Message = errorMsg, Response = ResponseCode.Exception };
        }
    }
}
