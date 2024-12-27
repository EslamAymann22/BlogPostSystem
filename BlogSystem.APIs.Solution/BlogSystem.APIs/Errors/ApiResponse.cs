
namespace BlogSystem.APIs.Errors
{
    public class ApiResponse
    {

        public int StatusCode {  get; set; }
        public string? ErrorMessage { get; set; }

        public ApiResponse(int StatusCode, string? ErrorMessage = null) {
            this.StatusCode = StatusCode;
            this.ErrorMessage= ErrorMessage?? GetErrorMessageWithStatusCode(StatusCode);
            
        }

        private string? GetErrorMessageWithStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "You are not authorized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
