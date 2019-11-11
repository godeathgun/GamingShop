using Model.EF;
using System.Collections.Generic;
using System.Linq;

namespace Model.ModelController
{
    public class MoProductCategoryController
    {
        GamingShopDbContext db;
        public MoProductCategoryController()
        {
            db = new GamingShopDbContext();
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x=>x.DisplayOrder).ToList();
        }
        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
    }
}
