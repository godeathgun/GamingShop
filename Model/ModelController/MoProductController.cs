using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace Model.ModelController
{
    public class MoProductController
    {
        private GamingShopDbContext db = null;

        public MoProductController()
        {
            db = new GamingShopDbContext();
        }

        public List<Product> ListAll()
        {
            return db.Products.Where(x => x.Status == true).ToList();
        }


        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }


        public bool Update(Product entity)
        {
            try
            {
                var product = db.Products.Find(entity.ID);
                product.Name = entity.Name;
                product.Code = entity.Code;
                product.CategoryID = entity.CategoryID;
                product.Price = entity.Price;
                product.Image = entity.Image;
                product.ModifiedBy = entity.ModifiedBy;
                product.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool ChangeStatus(long id)
        {
            var product = db.Products.Find(id);
            product.Status = !product.Status;
            db.SaveChanges();
            return (product.Status);
        }

        public void UpdateMoreImages(long id, string images)
        {
            var product = db.Products.Find(id);
            product.MoreImages = images;
            db.SaveChanges();
        }

        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public List<Product> listAllProduct()
        {
            object[] sqlParams =
            {
                
            };
            return db.Database.SqlQuery<Product>("sp_listAllProduct", sqlParams).ToList();
        }
        public Product GetByID(long id)
        {
            return db.Products.Find(id);
        }

        public long Insert(Product entity)
        {
            db.Products.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long CreateProduct(string name,long? categoryid,string code,string metatitle,string description,string image,
            decimal? price, decimal? promotionprice,int quantity,string detail,string createdby,bool status)
        {
            object[] sqlParams =
            {
                new SqlParameter("@Name",name),
                new SqlParameter("@CategoryId",categoryid),
                new SqlParameter("@Code",code),
                new SqlParameter("@MetaTitle",metatitle),
                new SqlParameter("@Description",description),
                new SqlParameter("@Image",image),
                new SqlParameter("@Price",price),
                new SqlParameter("@PromotionPrice",promotionprice),
                new SqlParameter("@Quantity",quantity),
                new SqlParameter("@Detail",detail),
                new SqlParameter("@CreatedBy",createdby),
                new SqlParameter("@Status", status)
            };
            long res = db.Database.ExecuteSqlCommand("sp_CreateProduct @Name,@CategoryId,@Code,@MetaTitle,@Description,@Image,@Price,@PromotionPrice,@Quantity,@Detail,@CreatedBy,@Status", sqlParams);
            return res;
        }
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

        public IEnumerable<Product> ListByCategoryId(long categoryID, ref int totalRecord, int page, int pageSize)
        {
            var query = db.Products.Where(x => x.CategoryID == categoryID);
            totalRecord = query.Count();
            return query.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
        }
        public IEnumerable<Product> Search(string keyword, ref int totalRecord, int page, int pageSize)
        {
            var query = db.Products.Where(x => x.Name.Contains(keyword));
            totalRecord = query.Count();
            return query.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
        }
        //public List<ProductViewModel> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        //{
        //    totalRecord = db.Products.Where(x => x.CategoryID == categoryID).Count();
        //    var model = (from a in db.Products
        //                 join b in db.ProductCategories
        //                 on a.CategoryID equals b.ID
        //                 where a.CategoryID == categoryID
        //                 select new
        //                 {
        //                     CateMetaTitle = b.MetaTitle,
        //                     CateName = b.Name,
        //                     CreatedDate = a.CreatedDate,
        //                     ID = a.ID,
        //                     Images = a.Image,
        //                     Name = a.Name,
        //                     MetaTitle = a.MetaTitle,
        //                     Price = a.Price
        //                 }).AsEnumerable().Select(x => new ProductViewModel()
        //                 {
        //                     CateMetaTitle = x.MetaTitle,
        //                     CateName = x.Name,
        //                     CreatedDate = x.CreatedDate,
        //                     ID = x.ID,
        //                     Images = x.Images,
        //                     Name = x.Name,
        //                     MetaTitle = x.MetaTitle,
        //                     Price = x.Price
        //                 });
        //    model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //    return model.ToList();
        //}
    }
}