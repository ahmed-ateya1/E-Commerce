using System.Net;

namespace E_Commerce.Core.Dtos
{
    public class ServiceResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; } = false;
        public object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
