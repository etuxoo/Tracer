using Microsoft.AspNetCore.Mvc;

namespace TraceService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/health")] // Support path versioning
    [ApiVersion("1.0")]
    public class HealthCheckController : ApiControllerBase
    {

    }
}
