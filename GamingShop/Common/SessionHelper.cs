using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamingShop.Common
{
    public class SessionHelper
    {
        public static void SetSession(UserLogin session)
        {
            HttpContext.Current.Session["loginSession"] = session;
        }
        public static UserLogin GetSession()
        {
            var session = HttpContext.Current.Session["loginSession"];
            if (session == null)
                return null;
            else
            {
                return session as UserLogin;
            }
        }
    }
}