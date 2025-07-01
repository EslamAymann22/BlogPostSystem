namespace BlogSystem.APIs.Errors
{
    public class ApiExceptionResponses : ApiResponse
    {

        public string? Details {  get; set; }

        public ApiExceptionResponses(int StatusCode , string? ErrorMessage= null , string? Details = null) 
            : base(StatusCode, ErrorMessage)
        {
            this.Details = Details;
        }

    }
}
