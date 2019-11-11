using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ModelController
{
    public class MoOrderDetailController
    {
        GamingShopDbContext db = null;
        public MoOrderDetailController()
        {
            db = new GamingShopDbContext();
        }
        public bool CancelOrder(long id)
        {
            try
            {
                var orderdetail = db.OrderDetails.Find(id);
                orderdetail.Order.Status = 4;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public OrderDetail ViewDetail(long id)
        {
            return db.OrderDetails.Where(x => x.OrderID == id).SingleOrDefault();
        }
        public bool Insert(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<OrderDetail> GetByUser(long userID)
        {
            return db.OrderDetails.Where(x => x.Order.CustomerID == userID).ToList();
        }

        public IEnumerable<OrderDetail> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<OrderDetail> model = db.OrderDetails;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.OrderID.ToString().Contains(searchString) || x.Order.User.UserName.Contains(searchString));
            }
            return model.OrderByDescending(x => x.Order.CreatedDate).ToPagedList(page, pageSize);
        }

        public int GetStatus(long id)
        {
            var order = db.Orders.Find(id);
            return order.Status;
        }

        public bool ChangeStatus(int status,long id)
        {
            try
            {
                var order = db.Orders.Find(id);
                order.Status = status;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var orderdetail = db.OrderDetails.Find(id);
                var order = db.Orders.Find(id);
                db.OrderDetails.Remove(orderdetail);
                db.Orders.Remove(order);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
