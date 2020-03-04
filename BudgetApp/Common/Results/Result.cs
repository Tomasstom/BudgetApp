using System;
using System.Collections.Generic;

namespace BudgetApp.Common.Results
{
    public class Result
    {
        protected Result()
        {
        }

        public Result(ResultStatus status, string message = null, IReadOnlyDictionary<string, string> details = null)
        {
            Status = status;
            Message = message;
            Details = details;
        }

        public ResultStatus Status { get; }
        public string Message { get; }
        public IReadOnlyDictionary<string, string> Details { get; }
        
        public bool Succeeded => Status == ResultStatus.Ok;
        
        public static Result Ok() => new Result(ResultStatus.Ok);
        public static Result<T> Ok<T>(T value) => new Result<T>(value);
        public static Result NotFound(string message) => new Result(ResultStatus.NotFound, message);
        public static Result NotValid(string message, IReadOnlyDictionary<string, string> details = null) 
            => new Result(ResultStatus.NotValid, message, details);
        public static Result NotAuthorized(string message) => new Result(ResultStatus.NotAuthorized, message);
        public static Result NotSpecified(string message) => new Result(ResultStatus.NotSpecified, message);
    }
    
    public class Result<T>
    {
        public Result(T data)
        {
            _data = data;
            Status = ResultStatus.Ok;
        }
        
        public Result(ResultStatus status, string message = null, IReadOnlyDictionary<string, string> details = null)
        {
            Status = status;
            Message = message;
            Details = details;
        }

        private readonly T _data;

        public ResultStatus Status { get; }
        public string Message { get; }
        public IReadOnlyDictionary<string, string> Details { get; }
        
        public bool Succeeded => Status == ResultStatus.Ok;

        public static implicit operator Result<T>(Result result)
        {
            return new Result<T>(result.Status, result.Message, result.Details);
        }

        public T Data
        {
            get
            {
                if (Status != ResultStatus.Ok) 
                    throw new InvalidOperationException("Operation failed, cannot retrieve value.");
                return _data;
            }
        }
    }
}