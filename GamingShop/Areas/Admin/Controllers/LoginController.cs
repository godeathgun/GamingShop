using GamingShop.Common;
using GamingShop.Areas.Admin.Models;
using Model.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamingShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public string USER_SESSION { get; private set; }

        // GET: Admin/Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Login(LoginModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var dao = new MoUserController();
        //        var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
        //        if (result==1)
        //        {
        //            var user = dao.GetById(model.UserName);
        //            var userSession = new UserLogin();
        //            userSession.UserName = user.UserName;
        //            userSession.UserID = user.ID;
        //            Session.Add(CommonConstants.USER_SESSION, userSession);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else if(result==0)
        //        {
        //            ModelState.AddModelError("", "Account does not exist.");
        //        }
        //        else if(result==-1)
        //        {
        //            ModelState.AddModelError("", "Account does not active.");
        //        }
        //        else if(result==-2)
        //        {
        //            ModelState.AddModelError("", "Wrong password.");
        //        }
        //    }
        //    return View("Index");
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
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
            return View("Index");
        }
    }
}