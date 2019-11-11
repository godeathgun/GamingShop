using Model.ModelController;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Linq;

namespace GamingShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Prodcut
        public ActionResult Index()
        {
            ViewBag.Slides = new MoSlideController().ListAll();
            return View();
        }
        public JsonResult ListName(string q)
        {
            var data = new MoProductController().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            },JsonRequestBehavior.AllowGet); 
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

        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new MoProductCategoryController().ListAll();
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult Menu()
        {
            var model = new MoProductCategoryController().ListAll();
            return PartialView(model);
        }

        public ActionResult Search(string keyword, int page = 1)
        {
            int totalRecord = 0;
            int pageSize = 12;
            var model = new MoProductController().Search(keyword, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            ViewBag.Keyword = keyword;
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        public ActionResult Category(long cateId, int page=1)
        {
            var category = new MoProductCategoryController().ViewDetail(cateId);
            ViewBag.Category = category;

            int totalRecord = 0;
            int pageSize = 12;
            var model = new MoProductController().ListByCategoryId(cateId, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        public ActionResult Detail(long Id)
        {
            var product = new MoProductController().ViewDetail(Id);
            return View(product);
        }
    }
}