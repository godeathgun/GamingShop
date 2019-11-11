using GamingShop.Common;
using GamingShop.Models;
using Model.ModelController;
using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace GamingShop.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private const string ForgotPasswordSession = "ForgotPasswordSession";

        // GET: ForgotPassword
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var user = new MoUserController().CheckEmail(email);
            if (!user)
            {
                ModelState.AddModelError("", "Email is not valid.");
            }
            Random random = new Random();
            var forgotpassword = new ForgotPasswordModel();
            forgotpassword.Code = random.Next(100000, 999999).ToString();
            forgotpassword.Email = email;
            Session.Add(CommonConstants.ForgotPasswordSession, forgotpassword);
            SendConfirmMail(email, forgotpassword.Code);
            return View("ConfirmCode");
        }

        [HttpPost]
        public ActionResult SendConfirmMail(string email, string code)
        {
            var mail = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("living2bred@gmail.com", "025908560@bc"),
                EnableSsl = true
            };
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("living2bred@gmail.com");
            message.Subject = "Confirmation Email";
            message.Body = "Your code to verify account: " + code;
            try
            {
                mail.Send(message);
            }
            catch { }
            return View();
        }

        public ActionResult ConfirmCode(string code)
        {
            if (ModelState.IsValid)
            {
                var forgotpasswordsession = (ForgotPasswordModel)Session[GamingShop.Common.CommonConstants.ForgotPasswordSession];
                if (code == forgotpasswordsession.Code)
                {
                    return View("NewPassword");
                }
                else
                {
                    ModelState.AddModelError("", "Code is not valid");
                    return View("ConfirmCode");
                }
            }
            return View("NewPassword");
        }

        public ActionResult NewPassword(string newpassword, string confirmpassword)
        {
            if (ModelState.IsValid)
            {
                var forgotpasswordsession = (ForgotPasswordModel)Session[GamingShop.Common.CommonConstants.ForgotPasswordSession];
                var userController = new MoUserController();
                var user = userController.GetByMail(forgotpasswordsession.Email);
                if (newpassword == confirmpassword)
                {
                    var encryptednewpassword = Encryptor.MD5Hash(newpassword);
                    user.Password = encryptednewpassword;
                    var result = userController.ChangePassword(user.ID, user);
                    if (result)
                    {
                        return View("Success");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Change password not succeeded");
                    return View("NewPassword");
                }
            }
            return View("Success");
        }
        public ActionResult Success()
        {
            return View();
        }
    }
}