using Model.EF;
using Model.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamingShop.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Admin/Content
          public ActionResult Index(string searchString, int page=1, int pageSize=5)
        {
            var productController = new MoContentController();
            var model = productController.ListAllPaging(searchString, page, pageSize);
            return View(model);
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new MoContentController().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag(null);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Content content)
        {
            var productController = new MoContentController();
            long id = productController.Insert(content);
            if (id > 0)
            {
                SetAlert("Thêm product thành công", "success");
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", "Thêm product không thành công");
            }
            SetViewBag();
            return View("Index");
        }

        [HttpPost]
        public ActionResult Edit(Content content)
        {
            var contentController = new MoContentController();

            var result = contentController.Update(content);
            if (result)
            {
                SetAlert("Sửa product thành công", "success");
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", "Update not succeeded");
            }
            SetViewBag();
            return View("Index");
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var contentController = new MoContentController();
            var content = contentController.GetByID(id);
            SetViewBag();
            return View();
        }

        public void SetViewBag(long? selectedID=null)
        {
            var productController = new MoProductController();
            ViewBag.ProductID = new SelectList(productController.ListAll(), "ID", "Name", selectedID);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new MoContentController().Delete(id);
            return RedirectToAction("Index");
        }
    }
}