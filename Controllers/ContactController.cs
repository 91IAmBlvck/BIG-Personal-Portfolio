using BIG.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace BIG.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Send(ContactFormModel model)
        {
            if (ModelState.IsValid)
                return RedirectToAction("Index", "Home");

            var fromEmail = "gamabandile91@gmail.com";
            var appPassword = "lued onfx bihc byzx";

            var mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add("gamabandile91@gmail.com");

            mail.Subject = model.Subject;
            mail.Body = 
                $"Name: {model.FullName}\n" +
                $"Email {model.Email}\n" +
                $"Phone: {model.PhoneNumber}\n\n" +
                $"Message: \n {model.Message}";

            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmail, appPassword);
            smtp.EnableSsl = true;

            smtp.Send(mail);

            return RedirectToAction("Index", "Home");
        }

    }
}
