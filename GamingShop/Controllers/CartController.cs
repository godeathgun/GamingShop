using GamingShop.Models;
using Model.EF;
using Model.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace GamingShop.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if(cart!=null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpGet]
        public ActionResult Checkout()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            var session = (UserLogin)Session[GamingShop.Common.CommonConstants.USER_SESSION];
            if (session != null)
            {
                var userController = new MoUserController();
                var user = userController.GetById(session.UserName);
                ViewBag.User = user;
            }
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Checkout(string shipName,string mobile, string address, string email)
        {
            var session = (UserLogin)Session[GamingShop.Common.CommonConstants.USER_SESSION];
            var userController = new MoUserController();
            var order = new Order();
            if (session!=null)
            {
                var user = userController.GetById(session.UserName);
                order.ID = user.ID;
                order.CreatedDate = DateTime.Now;
                order.ShipAddress = user.Address;
                order.ShipName = user.Name;
                order.ShipEmail = user.Email;
                order.ShipMobile = user.Phone;

            }
            else
            {
                order.CreatedDate = DateTime.Now;
                order.ShipAddress = address;
                order.ShipName = shipName;
                order.ShipEmail = email;
                order.ShipMobile = mobile;
            }
            try
            {
                var id = new MoOrderController().Insert(order);
                var cart = (List<CartItem>)Session[CartSession];
                var detail = new MoOrderDetailController();

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Quantity = item.Quantity;
                    detail.Insert(orderDetail);
                }
            }
            catch (Exception ex)
            {
                return Redirect("/CheckoutError");
            }
            return RedirectToAction("Success");
        }
        public ActionResult Success()
        {
            return View();
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sesstionCart = (List<CartItem>)Session[CartSession];
            foreach(var item in sesstionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                    item.Quantity = jsonItem.Quantity;
            }
            Session[CartSession] = sesstionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(long id)
        {
            var sesstionCart = (List<CartItem>)Session[CartSession];
            sesstionCart.RemoveAll(x => x.Product.ID == id);
            Session[CartSession] = sesstionCart;
            return Json(new
            {
                status = true
            });
        }

        public ActionResult AddItem(long productId, int quantity)
        {
            var product = new MoProductController().ViewDetail(productId);
            var cart = Session[CartSession];
            if(cart!=null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //Tạo mới đối tượng item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[CartSession] = list;
            }
            else
            {
                //Tạo mới đối tượng item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gán
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
    }
}