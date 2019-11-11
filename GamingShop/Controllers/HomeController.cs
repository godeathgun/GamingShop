using GamingShop.Common;
using GamingShop.Models;
using Model.ModelController;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace GamingShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Slides = new MoSlideController().ListAll();
            var productController = new MoProductController();
            ViewBag.NewProducts = productController.ListNewProduct(4);
            ViewBag.ListAllProduct = productController.listAllProduct();
            var contentController = new MoContentController().ListAll();
            return View(contentController);
        }
        [ChildActionOnly]
        public ActionResult Footer()
        {
            var model = new MoFooterController().GetFooter();
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
    }
}