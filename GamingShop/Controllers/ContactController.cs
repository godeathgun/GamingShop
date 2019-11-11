using Model.ModelController;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace GamingShop.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            var model = new MoContactController().ListAll();
            return View(model);
        }
        // GET: Contact
        [HttpGet]
        public ActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(string Subject, string Body, HttpPostedFileBase file)
        {
            var mail = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("living2bred@gmail.com", "025908560@bc"),
                EnableSsl = true
            };
            var message = new MailMessage();
            message.To.Add(new MailAddress("phatpham01228@gmail.com"));
            message.From = new MailAddress("living2bred@gmail.com");
            message.Subject = Subject;
            message.Body = Body;
            int flag = 0;
            if (file != null && file.ContentLength > 0)
            {
                string path = Path.GetFileName(file.FileName);
                message.Attachments.Add(new Attachment(file.InputStream, path));
            }
            try
            {
                mail.Send(message);
                flag = 1;
            }
            catch { }
            if (flag > 0)
            {
                ViewBag.Success = "Thank you for your feedback.";
                flag = 0;
            }
            return View();
        }
    }
}