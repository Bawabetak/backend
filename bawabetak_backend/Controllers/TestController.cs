using Microsoft.AspNetCore.Mvc;

namespace Bawabetak.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Bawabetak API is working fine!" });
        }
    }
}