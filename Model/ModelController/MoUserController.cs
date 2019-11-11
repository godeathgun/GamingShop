using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList.Mvc;
using PagedList;
using System.Data.SqlClient;

namespace Model.ModelController
{
    public class MoUserController
    {
        GamingShopDbContext db = null;
        public MoUserController()
        {
            db = new GamingShopDbContext();
        }

        public bool CheckUserName(string username)
        {
            return db.Users.Count(x => x.UserName == username) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return (user.Status);
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Edit(long id,User entity)
        {
            try
            {
                var user = db.Users.Find(id);
                user.Name = entity.Name;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                user.Phone = entity.Phone;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool ChangePassword(long id,User entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    var user = db.Users.Find(id);
                    user.Password = entity.Password;
                }
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            { return false; }
        }
        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Name = entity.Name;
                user.Address = entity.Address;    
                user.Email = entity.Email;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                user.Phone = entity.Phone;
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
 

        public IEnumerable<User> ListAllPaging(string searchString,int page, int pageSize)
        {
            IQueryable<User> model = db.Users;
            if(!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }
        public User GetByMail(string email)
        {
            return db.Users.SingleOrDefault(x => x.Email == email);
        }
        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }
        public List<User> ListAll()
        {
            return db.Users.Where(x => x.GroupID == "admin").OrderByDescending(x => x.CreatedDate).ToList();
        }
        //public int Login(string userName, string passWord)
        //{
        //    var result = db.Users.SingleOrDefault(x => x.UserName == userName);
        //    if (result == null)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        if(result.Status==false)
        //        {
        //            return -1;
        //        }
        //        else
        //        {
        //            if (result.Password == passWord)
        //                return 1;
        //            else
        //                return -2;
        //        }
        //    }
        //}
        public int Login(string username,string password)
        {
            object[] sqlParams =
            {
                new SqlParameter("UserName",username),
                new SqlParameter("PassWord",password),
            };
            var result = db.Database.SqlQuery<int>("sp_Login @UserName,@Password", sqlParams).SingleOrDefault();
            return result;
        }
    }
}
