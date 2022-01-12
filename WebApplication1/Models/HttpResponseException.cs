using System;
namespace WebApplication1.Models {
    [Serializable]
    public class HttpResponseException : Exception {
        public HttpResponseException(int statusCode = StatusCodes.Status400BadRequest,string? error = null) =>
        (StatusCode, Error) = (statusCode, error);

        public int StatusCode { get; }

        public string? Error { get; }
    }
}
