namespace BlogSystem.APIs.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {

        public List<string> errors { get; set; }
        public ApiValidationErrorResponse() : base(400)
        {
            errors = new List<string>();
        }
    }
}
