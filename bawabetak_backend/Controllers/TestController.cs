using Microsoft.AspNetCore.Mvc;

namespace Bawabetak.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IEmailSenderHelper _emailSender;

        public TestController(IEmailSenderHelper emailSender)
        {
            _emailSender = emailSender;
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
    }
}