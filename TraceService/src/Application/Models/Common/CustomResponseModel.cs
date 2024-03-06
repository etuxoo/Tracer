namespace TraceService.Application.Models
{
    public class CustomResponseModel<T>
    {
        public T Data { get; set; }
        public ErrorModel Error { get; set; } = new();
    }
}
