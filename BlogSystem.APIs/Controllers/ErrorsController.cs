using BlogSystem.APIs.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.APIs.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    public class ErrorsController : ControllerBase
    {

        public ActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code));
        }

    }
}
