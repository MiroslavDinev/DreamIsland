namespace DreamIsland.Controllers
{
    using System;
    using System.Net.Mail;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using DreamIsland.Models;
    using static WebConstants.GlobalMessages;

    public class ContactController : Controller
    {
        private const string AdminEmailAddress = "admin.dreamisland@dir.bg";

        [Authorize]
        public IActionResult Book()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Book(ContactFormViewModel contact)
        {
            if (!ModelState.IsValid)
            {
                return this.View(contact);
            }

            MailAddress to = new MailAddress(AdminEmailAddress);
            MailAddress from = new MailAddress(contact.Email);

            MailMessage mailMessage = new MailMessage(from, to);

            mailMessage.Subject = contact.Subject;
            mailMessage.Body = contact.Content;

            SmtpClient client = new SmtpClient("mail.dir.bg", 587);

            try
            {
                client.Send(mailMessage);
            }
            catch (SmtpException ex)
            {

                Console.WriteLine(ex.Message);
            }

            this.TempData[SuccessMessageKey] = SuccessContactMessage;

            return RedirectToAction("Index", "Home");
        }
    }
}
