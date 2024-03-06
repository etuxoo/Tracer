using TraceService.Application.Interfaces;

namespace TraceService.Application.Models
{
    public class Result //: IMapFrom<TraceService.Client.Models.Result>
    {
        public Result()
        {
        }

        internal Result(bool succeeded, string error)
            : this(succeeded, 0, error)
        {
        }

        internal Result(bool succeeded, int code, string error)
        {
            this.Succeeded = succeeded;
            this.Code = code;
            this.Error = error;
        }

        public bool Succeeded { get; set; }

        public int Code { get; set; }

        public string Error { get; set; }

        public static Result Success()
        {
            return new(true, string.Empty);
        }

        public static Result Failure(string error)
        {
            return new(false, error);
        }

        public static Result Failure(int code, string error)
        {
            return new(false, code, error);
        }
    }

    public class Result<T> :IResult<T> where T : class 
    {
        internal Result(T data, bool succeeded, string error)
            : this(data, succeeded, 0, error)
        {
        }

        internal Result(T data, bool succeeded, int code, string error)
        {
            this.Succeeded = succeeded;
            this.Code = code;
            this.Error = error;
            this.Data = data;
        }

        public bool Succeeded { get; set; }

        public int Code { get; set; }

        public string Error { get; set; }

        public T Data { get; set; }

        public static Result<T> Success(T data)
        {
            return new(data, true, string.Empty);
        }

        public static Result<T> Failure(string error)
        {
            return new(null, false, error);
        }

        public static Result<T> Failure(int code, string error)
        {
            return new(null, false, code, error);
        }
    }
}
