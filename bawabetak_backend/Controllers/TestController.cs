using bawabetak_backend.Helpers.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Bawabetak.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IEmailSenderHelper _emailSender;
        private readonly ICacheHelper _cacheManager;
        private readonly IBackgroundJobClient _backgroundJob; 

        public TestController(
            IEmailSenderHelper emailSender,
            ICacheHelper cacheManager,
            IBackgroundJobClient backgroundJob)
        {
            _emailSender = emailSender;
            _cacheManager = cacheManager;
            _backgroundJob = backgroundJob;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Bawabetak API is working fine!" });
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromQuery] string targetEmail)
        {
            string subject = "تجربة إرسال إيميل من Bawabetak! 🚀";
            string htmlMessage = "<h1>مرحباً بك في بوابة تك!</h1><p>إذا وصلك هذا الإيميل، فكل شيء يعمل بنجاح تام.</p>";

            await _emailSender.SendEmailAsync(targetEmail, subject, htmlMessage);

            return Ok(new { message = "تم إرسال الإيميل التجريبي بنجاح! شيك على الـ Inbox." });
        }

        [HttpPost("redis-cache/set")]
        public async Task<IActionResult> SetRedisCache([FromQuery] string key, [FromBody] object value, [FromQuery] int? ttlInMinutes = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                return BadRequest(new { success = false, message = "Redis Key cannot be empty!" });

            TimeSpan? ttl = ttlInMinutes.HasValue ? TimeSpan.FromMinutes(ttlInMinutes.Value) : null;

            await _cacheManager.SetAsync(key, value, ttl);

            return Ok(new { success = true, message = $"Data written to Redis successfully with Key: '{key}'" });
        }

        [HttpGet("redis-cache/get")]
        public async Task<IActionResult> GetRedisCache([FromQuery] string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return BadRequest(new { success = false, message = "Redis Key cannot be empty!" });

            var cachedData = await _cacheManager.GetAsync<object>(key);

            if (cachedData == null)
                return NotFound(new { success = false, message = $"Redis Key '{key}' not found or has expired!" });

            return Ok(new { success = true, source = "Redis Server (Upstash) 🚀", data = cachedData });
        }

        [HttpPost("hangfire/fire-and-forget")]
        public IActionResult TriggerBackgroundJob([FromQuery] string taskName)
        {
            if (string.IsNullOrWhiteSpace(taskName))
                return BadRequest(new { success = false, message = "Task name cannot be empty!" });

            _backgroundJob.Enqueue(() => Console.WriteLine($"--> Hangfire executing: {taskName} in background at {DateTime.UtcNow}"));

            return Ok(new { success = true, message = "Task enqueued successfully via Hangfire (Redis Storage)!" });
        }
      
    }
}