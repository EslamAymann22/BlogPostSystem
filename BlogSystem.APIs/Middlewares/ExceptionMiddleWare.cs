using BlogSystem.APIs.Errors;
using System.Linq.Expressions;
using System.Net;
using System.Text.Json;

namespace BlogSystem.APIs.Middlewares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleWare(RequestDelegate Next 
                                   ,ILogger<ExceptionMiddleWare> logger
                                   ,IHostEnvironment env)
        {
            _next = Next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


                var Response = (_env.IsDevelopment())
                                                    ? new ApiExceptionResponses(500, ex.Message, ex.StackTrace)
                                                    : new ApiExceptionResponses(500);

                var option = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(Response, option));

            }

        }

    }
}
