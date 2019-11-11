using GamingShop.Common;
using GamingShop.Models;
using Model.EF;
using Model.ModelController;
using PoliteCaptcha;
using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace GamingShop.Controllers
{
    public class UserController : Controller
    {
        private const string ForgotPasswordSession = "ForgotPasswordSession";

        public ActionResult Index()
        {
            var session = (UserLogin)Session[GamingShop.Common.CommonConstants.USER_SESSION];
            var model = new MoUserController().GetById(session.UserName);
            return View(model);
        }


        [HttpGet]
        public ActionResult ChangePassword(int id)
        {
            var session = (UserLogin)Session[GamingShop.Common.CommonConstants.USER_SESSION];
            var model = new MoUserController().GetById(session.UserName);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(User user, string password, string newpassword, string confirmpassword)
        {
            var session = (UserLogin)Session[GamingShop.Common.CommonConstants.USER_SESSION];
            long id = session.UserID;
            var userController = new MoUserController();
            ViewBag.Success = false;
            if (user.Password == password)
            {
                if (newpassword == confirmpassword)
                {
                    var encryptednewpassword = Encryptor.MD5Hash(newpassword);
                    user.Password = encryptednewpassword;
                    var result = userController.ChangePassword(id, user);
                    if (result)
                    {
                        return RedirectToAction("ChangePassword/" + session.UserID, "User");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Confirm password is wrong");
                }
            }
            else
            {
                ModelState.AddModelError("", "Change password not succeeded");
            }
            return View("ChangePassword");
        }

        public ActionResult OrdersofCustomer()
        {
            var session = (UserLogin)Session[GamingShop.Common.CommonConstants.USER_SESSION];
            var orderController = new MoOrderDetailController().GetByUser(session.UserID);
            return View(orderController);
        }

        [HttpGet]
        public ActionResult ViewDetailOrder(long id)
        {
            var orderDetail = new MoOrderDetailController().ViewDetail(id);
            return View(orderDetail);
        }

        [HttpPost]
        public ActionResult ViewDetailOrder(OrderDetail orderDetail)
        {
            var result = new MoOrderDetailController().CancelOrder(orderDetail.OrderID);
            if(result)
            {
                return RedirectToAction("OrdersofCustomer", "User");
            }
            else
            {
                ModelState.AddModelError("", "Error");
            }
            return View("OrdersofCustomer");
        }

        public ActionResult Edit(int id)
        {
            var user = new MoUserController().ViewDetail(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            var session = (UserLogin)Session[GamingShop.Common.CommonConstants.USER_SESSION];
            long id = session.UserID;
            var userController = new MoUserController();
            var result = userController.Edit(id, user);
            if (result)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", "Update not succeeded");
            }
            return View("Index");
        }

        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateSpamPrevention]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userController = new MoUserController();
                if (userController.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Invalid user name");
                }
                else if (userController.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email has already been registed");
                }
                else
                {
                    var user = new User();
                    user.Name = model.Name;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.Phone = model.Phone;
                    user.Address = model.Address;
                    user.Email = model.Email;
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;
                    user.GroupID = "customer";
                    var result = userController.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Registerd Success";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error occur");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new MoUserController().Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = new MoUserController().GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Account does not exist.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Account does not active.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Wrong password.");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }
    }
}