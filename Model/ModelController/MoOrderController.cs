using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ModelController
{
    public class MoOrderController
    {
        GamingShopDbContext db = null;
        public MoOrderController()
        {
            db = new GamingShopDbContext();
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }

        public int GetStatus(long id)
        {
            var order = db.Orders.Find(id);
            return order.Status;
        }
        public Order ViewDetail(long id)
        {
            return db.Orders.Where(x => x.ID == id).SingleOrDefault();
        }
        public bool ChangeStatus(int status, long id)
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

    }
}
