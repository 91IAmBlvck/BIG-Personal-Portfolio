//using Microsoft.Extensions.Configuration;
//using BIG.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using System.Net.Mail;
//using System.Threading.Tasks;

//namespace BIG.Controllers
//{
//    public class ContactController : Controller
//    {
//        private readonly IConfiguration _configuration;

//        public ContactController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        //[HttpPost]
//        //public IActionResult Send(ContactFormModel model)
//        //{
//        //    try
//        //    {
//        //        if (!ModelState.IsValid)
//        //        {
//        //            return RedirectToAction("Index", "Home");
//        //        }

//        //        var fromEmail = _configuration["EmailSettings:Email"];
//        //        var appPassword = _configuration["EmailSettings:AppPassword"];

//        //        var mail = new MailMessage();

//        //        mail.From = new MailAddress(fromEmail);
//        //        mail.To.Add("gamabandile91@gmail.com");
//        //        mail.Subject = model.Subject;

//        //        mail.Body =
//        //            $"Name: {model.FullName}\n" +
//        //            $"Email: {model.Email}\n" +
//        //            $"Phone: {model.PhoneNumber}\n\n" +
//        //            $"Message:\n{model.Message}";

//        //        var smtp = new SmtpClient("smtp.gmail.com", 587)
//        //        {
//        //                Credentials = new NetworkCredential(fromEmail, appPassword),
//        //                EnableSsl = true,
//        //                Timeout = 20000
//        //        };


//        //        //smtp.Credentials = new NetworkCredential(fromEmail, appPassword);
//        //        //smtp.EnableSsl = true;

//        //        smtp.Send(mail);

//        //        TempData["SuccessMessage"] = "Message sent successfully!";
//        //        return Redirect("/#contact");

//        //        //return RedirectToAction("Index", "Home");

//        //        //return Content("Email sent successfully!");
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        TempData["ErrorMessage"] = ex.Message;
//        //        return Redirect("/#contact");

//        //        //return RedirectToAction("Index", "Home");
//        //    }
//        //}


//        [HttpPost]
//        public async Task<IActionResult> Send(ContactFormModel model)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return Redirect("/#contact");
//                }

//                var fromEmail = _configuration["EmailSettings:Email"];
//                var appPassword = _configuration["EmailSettings:AppPassword"];

//                var mail = new MailMessage
//                {
//                    From = new MailAddress(fromEmail),
//                    Subject = model.Subject,
//                    Body =
//                        $"Name: {model.FullName}\n" +
//                        $"Email: {model.Email}\n" +
//                        $"Phone: {model.PhoneNumber}\n\n" +
//                        $"Message:\n{model.Message}"
//                };

//                mail.To.Add("gamabandile91@gmail.com");

//                using var smtp = new SmtpClient("smtp.gmail.com", 587)
//                {
//                    Credentials = new NetworkCredential(fromEmail, appPassword),
//                    EnableSsl = true,
//                    Timeout = 30000
//                };

//                await smtp.SendMailAsync(mail);

//                TempData["SuccessMessage"] = "Message sent successfully!";
//                return Redirect("/#contact");
//            }
//            catch (Exception ex)
//            {
//                TempData["ErrorMessage"] = ex.Message;
//                return Redirect("/#contact");
//            }
//        }
//    }
//}



//using BIG.Models;
//using System.Net;
//using Microsoft.AspNetCore.Mvc;
//using Resend;

//namespace BIG.Controllers
//{
//    public class ContactController : Controller
//    {
//        private readonly IResend _resend;

//        public ContactController(IResend resend)
//        {
//            _resend = resend;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Send(ContactFormModel model)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return Redirect("/#contact");
//                }

//                var message = new EmailMessage();

//                message.From = "onboarding@resend.dev";
//                message.To.Add("gamabandile91@gmail.com");

//                message.Subject = model.Subject;

//                message.HtmlBody = $@"
//                    <h2>New Portfolio Contact Message</h2>

//                    <p><strong>Name:</strong> {model.FullName}</p>

//                    <p><strong>Email:</strong> {model.Email}</p>

//                    <p><strong>Phone:</strong> {model.PhoneNumber}</p>

//                    <p><strong>Message:</strong></p>

//                    <p>{model.Message}</p>
//                ";

//                await _resend.EmailSendAsync(message);

//                TempData["SuccessMessage"] = "Message sent successfully!";

//                return Redirect("/#contact");
//            }
//            catch (Exception ex)
//            {
//                TempData["ErrorMessage"] = ex.Message;

//                return Redirect("/#contact");
//            }
//        }
//    }
//}


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
                //var message = new EmailMessage
                //{
                //    From = "Portfolio <onboarding@resend.dev>",
                //    To = new string[] { "gamabandile91@gmail.com" },
                //    Subject = model.Subject ?? "New Contact Message",
                //    HtmlBody = $@"
                //        <h2>New Portfolio Contact Message</h2>
                //        <p><strong>Name:</strong> {model.FullName}</p>
                //        <p><strong>Email:</strong> {model.Email}</p>
                //        <p><strong>Phone:</strong> {model.PhoneNumber}</p>
                //        <p><strong>Message:</strong><br/>{model.Message}</p>
                //    "
                //};

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