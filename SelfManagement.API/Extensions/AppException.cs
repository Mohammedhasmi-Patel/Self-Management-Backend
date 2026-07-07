namespace SelfManagement.API.Extensions
{
    public class AppException : Exception
    {
        public int StatusCode { get; set; }

        public AppException(string message , int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message, 400) { }
        public class NotFoundException : AppException
        {
            public NotFoundException(string message) : base(message, 404) { }
        }

        public class UnauthorizedException : AppException
        {
            public UnauthorizedException(string message) : base(message, 401) { }
        }
        public class ConflictException : AppException
        {
            public ConflictException(string message) : base(message, 409) { }
        }

        public class InternalServerError : AppException
        {
            public InternalServerError(string message) : base(message,500)
            {
                
            }
        }
    }
}
