using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ModelController
{
    public class MoSlideController
    {
        GamingShopDbContext db = null;
        public MoSlideController()
        {
            db = new GamingShopDbContext();
        }
        public List<Slide> ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(x=>x.DisplayOrder).ToList();
        }
    }
}
