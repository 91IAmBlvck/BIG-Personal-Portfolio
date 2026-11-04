using BIG.Models;
using Microsoft.AspNetCore.Mvc;
using Resend;

namespace BIG.Controllers
{
    public class ContactController : Controller
    {
        private readonly IResend _resend;

        public ContactController(IResend resend)
        {
            _resend = resend;
        }

        [HttpPost]
        public async Task<IActionResult> Send(ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/#contact");
            }

            try
            {

                var message = new EmailMessage
                {
                    From = "Bandile Portfolio <onboarding@resend.dev>",
                    ReplyTo = model.Email,
                    To = new string[] { "gamabandile91@gmail.com" },
                    Subject = $"[Portfolio Contact] {model.Subject ?? "New Message"}",
                    HtmlBody = $@"
        <h2>New Portfolio Contact Message</h2>
        <p><strong>Name:</strong> {model.FullName}</p>
        <p><strong>Email:</strong> {model.Email}</p>
        <p><strong>Phone:</strong> {model.PhoneNumber}</p>
        <p><strong>Message:</strong><br/>{model.Message}</p>
    "
                };

                await _resend.EmailSendAsync(message);

                TempData["SuccessMessage"] = "Message sent successfully!";
            }
            catch
            {
                TempData["ErrorMessage"] = "Failed to send message.";
            }

            return Redirect("/#contact");
        }
    }
}