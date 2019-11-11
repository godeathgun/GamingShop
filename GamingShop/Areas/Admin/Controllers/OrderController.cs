using Model.ModelController;
using System.Web.Mvc;

namespace GamingShop.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var orderController = new MoOrderDetailController();
            var model = orderController.ListAllPaging(searchString, page, pageSize);

            return View(model);
        }

        public ActionResult Detail(long id)
        {
            var order = new MoOrderDetailController().ViewDetail(id);
            return View(order);
        }

        [HttpGet]
        public ActionResult ChangeStatus(long id)
        {
            var result = new MoOrderController().ViewDetail(id);
            return View(result);
        }

        [HttpPost]
        public ActionResult ChangeStatus(long id, int status)
        {
            var order = new MoOrderController();
            var result = order.ChangeStatus(status, id);
            if(result)
            {
                return RedirectToAction("Index", "Order");
            }
            else
            {
                ModelState.AddModelError("", "Error");
                RedirectToAction("ChangeStatus/" + id);
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new MoUserController().Delete(id);
            return RedirectToAction("Index");
        }
    }
}