using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ModelController
{
    public class MoContactController
    {
        GamingShopDbContext db = null;
        public MoContactController()
        {
            db = new GamingShopDbContext();
        }
        public Contact GetContact()
        {
            return db.Contacts.SingleOrDefault(x=>x.Status==true);
        }

        public List<Contact> ListAll()
        {
            object[] sqlParams =
            {

            };
            return db.Database.SqlQuery<Contact>("ListAllContact", sqlParams).ToList();
        }
    }
}
