using Model.EF;
using Model.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace GamingShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var productController = new MoProductController();
            var model = productController.ListAllPaging(searchString, page, pageSize);
            return View(model);
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new MoProductController().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public JsonResult LoadImages(long id)
        {
            var productController = new MoProductController();
            var product = productController.ViewDetail(id);
            var images = product.MoreImages;
            try
            {
                XElement xImages = XElement.Parse(images);
                List<string> listImagesReturn = new List<string>();
                foreach (XElement element in xImages.Elements())
                {
                    listImagesReturn.Add(element.Value);
                }
                return Json(new
                {
                    data = listImagesReturn
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveImages(long id, string images)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var listImages = serializer.Deserialize<List<string>>(images);
            XElement xElement = new XElement("Images");
            foreach (var item in listImages)
            {
                var subStringItem = "";
                int count = 0;
                for (int i = 0; i < item.Length; i++)
                {
                    char[] a = new char[item.Length];
                    a = item.ToArray();
                    if (a[i] == '/')
                        count++;
                    if (count == 3)
                    {
                        subStringItem = item.Substring(i);
                        break;
                    }
                }
                xElement.Add(new XElement("Image", subStringItem));
            }
            var productController = new MoProductController();
            try
            {
                productController.UpdateMoreImages(id, xElement.ToString());
                return Json(new
                {
                    status = true
                });
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Create(Product model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        new MoProductController().Insert(model);
        //        model.CreatedDate = DateTime.Now;
        //        return RedirectToAction("Index");
        //    }
        //    SetViewBag();
        //    return View();
        //}

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product model)
        {
            if (ModelState.IsValid)
            {
                var productController = new MoProductController();
                long res= productController.CreateProduct(model.Name,model.CategoryID,model.Code,model.MetaTitle,model.Description,model.Image,
                    model.Price,model.PromotionPrice,model.Quantity,model.Detail,model.CreatedBy,model.Status);
                model.CreatedDate = DateTime.Now;
                if (res>0)
                    return RedirectToAction("Index");
            }
            SetViewBag();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            var productController = new MoProductController();

            var result = productController.Update(product);
            if (result)
            {
                SetAlert("Sửa product thành công", "success");
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ModelState.AddModelError("", "Update not succeeded");
            }
            SetViewBag(product.CategoryID);
            return View("Index");
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var productController = new MoProductController();
            var product = productController.GetByID(id);
            SetViewBag(product.CategoryID);
            return View();
        }

        public void SetViewBag(long? selectedID = null)
        {
            var productcategoryController = new MoProductCategoryController();
            ViewBag.CategoryID = new SelectList(productcategoryController.ListAll(), "ID", "Name", selectedID);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new MoProductController().Delete(id);
            return RedirectToAction("Index");
        }
    }
}