using GamingShop.Common;
using Model.ModelController;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace GamingShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index(string searchString, int page=1,int pageSize=2)
        {
            var userController = new MoUserController();
            var model = userController.ListAllPaging(searchString,page,pageSize);
            return View(model);
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new MoUserController().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        // GET: Admin/User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/User/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var userController = new MoUserController();
                var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMd5Pas;
                user.GroupID = "admin";
                long id = userController.Insert(user);
                if (id > 0)
                {
                    SetAlert("Thêm user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user không thành công");
                }
            }
            return View("Index");
        }

        // GET: Admin/User/Edit/5
        public ActionResult Edit(int id)
        {
            var user = new MoUserController().ViewDetail(id);
            return View(user);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        public ActionResult Edit(User user)
        {
            var userController = new MoUserController();
            if (!string.IsNullOrEmpty(user.Password))
            {
                var encryptedMD5Pass = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMD5Pass;
            }

            var result = userController.Update(user);
            if (result)
            {
                SetAlert("Sửa user thành công", "success");
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", "Update not succeeded");
            }
            return View("Index");
        }

        // GET: Admin/User/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new MoUserController().Delete(id);
            return RedirectToAction("Index");
        }
    }
}
