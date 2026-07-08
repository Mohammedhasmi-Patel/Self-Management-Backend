

namespace SelfManagement.Application.DTO.Common
{
    public class ApiResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public bool Success { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
        {
            return new ApiResponse<T>()
            {
                Data = data,
                Message = message,
                Success = true
            };
        }

        public static ApiResponse<T> FailureResponse(string message)
        {
            return new ApiResponse<T>()
            {
                Data = default,
                Message = message,
                Success = false
            };
        }
    }
}
