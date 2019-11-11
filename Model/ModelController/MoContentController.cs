using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace Model.ModelController
{
    public class MoContentController
    {
        GamingShopDbContext db = null;
        public MoContentController()
        {
            db = new GamingShopDbContext();
        }
        public List<Content> ListAll()
        {
            return db.Contents.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public long Insert(Content entity)
        {
            db.Contents.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool ChangeStatus(long id)
        {
            var user = db.Contents.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return (user.Status);
        }
        public bool Update(Content entity)
        {
            try
            {
                var content = db.Contents.Find(entity.ID);
                content.Image = entity.Image;
                content.Product.Name = entity.Name;
                content.ProductID = entity.ID;
                content.ModifiedBy = entity.ModifiedBy;
                content.DisplayOrder = entity.DisplayOrder;
                content.ModifiedDate = DateTime.Now;
                content.Description = entity.Description;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }
        public bool Delete(int id)
        {
            try
            {
                var product = db.Contents.Find(id);
                db.Contents.Remove(product);
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
