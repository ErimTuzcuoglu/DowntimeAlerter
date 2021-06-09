using Microsoft.AspNetCore.Mvc;

namespace DowntimeAlerter.Domain.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApplicationControllerBase : ControllerBase
    {
    }
}