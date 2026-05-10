using Microsoft.Extensions.Configuration;
using BIG.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace BIG.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContactController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Send(ContactFormModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Home");
                }

                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var appPassword = _configuration["EmailSettings:AppPassword"];

                var mail = new MailMessage();

                mail.From = new MailAddress(fromEmail);
                mail.To.Add("gamabandile91@gmail.com");
                mail.Subject = model.Subject;

                mail.Body =
                    $"Name: {model.FullName}\n" +
                    $"Email: {model.Email}\n" +
                    $"Phone: {model.PhoneNumber}\n\n" +
                    $"Message:\n{model.Message}";

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                        Credentials = new NetworkCredential(fromEmail, appPassword),
                        EnableSsl = true,
                        Timeout = 20000
                };
                

                smtp.Credentials = new NetworkCredential(fromEmail, appPassword);
                smtp.EnableSsl = true;

                smtp.Send(mail);

                return Content("Email sent successfully!");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}