using System.Net;

namespace Domain.Exception;
public class CustomException : System.Exception
{
    public HttpStatusCode StatusCode { get; private set; }
    public string Message { get; private set; }
    public CustomException(HttpStatusCode statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}
