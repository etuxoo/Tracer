namespace TraceService.Application.Interfaces
{
    public interface IResult<T>
    {
        public bool Succeeded { get; set; }
        public int Code { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
}
