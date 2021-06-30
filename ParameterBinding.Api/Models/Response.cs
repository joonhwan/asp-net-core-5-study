namespace ParameterBinding.Api.Models
{
    public class Response<T>
    {
        public T Data { get; set; }
        public string[] Errors { get; set; } 
        public string Message { get; set; }
    }
}