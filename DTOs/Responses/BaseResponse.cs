using System.Net;

namespace Milvus_Vector_Database_API.DTOs.Responses
{
    public class BaseResponse(HttpStatusCode statusCode, Object data, String message)
    {
        public int? StatusCode { get; set; } = (int)statusCode;
        public object? Data { get; set; } = data;
        public string? Message { get; set; } = message;
    }
}
