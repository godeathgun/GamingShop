using System.Web.Mvc;
using System.Web.Routing;

namespace GamingShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
name: "Forgot Password",
url: "User/ForgotPassword",
defaults: new { controller = "User", action = "ForgotPassword", id = UrlParameter.Optional },
namespaces: new[] { "GamingShop.Controllers" }
);


            routes.MapRoute(
name: "Customer Edit Profile",
url: "User/Edit",
defaults: new { controller = "User", action = "Edit", id = UrlParameter.Optional },
namespaces: new[] { "GamingShop.Controllers" }
);

            routes.MapRoute(
name: "Customer Profile",
url: "User/Profile",
defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional },
namespaces: new[] { "GamingShop.Controllers" }
);

            routes.MapRoute(
name: "Customer Change Password",
url: "User/ChangePassword",
defaults: new { controller = "User", action = "ChangePassword", id = UrlParameter.Optional },
namespaces: new[] { "GamingShop.Controllers" }
);

            routes.MapRoute(
name: "Customer Orders",
url: "User/Order",
defaults: new { controller = "User", action = "OrdersofCustomer", id = UrlParameter.Optional },
namespaces: new[] { "GamingShop.Controllers" }
);

            routes.MapRoute(
 name: "Search",
 url: "Search",
 defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
 namespaces: new[] { "GamingShop.Controllers" }
);

            routes.MapRoute(
    name: "Logout",
    url: "Logout",
    defaults: new { controller = "User", action = "Logout", id = UrlParameter.Optional },
    namespaces: new[] { "GamingShop.Controllers" }
);
            routes.MapRoute(
        name: "Login",
        url: "Login",
        defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
        namespaces: new[] { "GamingShop.Controllers" }
    );
            routes.MapRoute(
         name: "Register",
         url: "Register",
         defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
         namespaces: new[] { "GamingShop.Controllers" }
     );
            routes.MapRoute(
          name: "Checkout Success",
          url: "Success",
          defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
          namespaces: new[] { "GamingShop.Controllers" }
      );
            routes.MapRoute(
            name: "Checkout",
            url: "Checkout",
            defaults: new { controller = "Cart", action = "Checkout", id = UrlParameter.Optional },
            namespaces: new[] { "GamingShop.Controllers" }
        );

            routes.MapRoute(
          name: "Cart",
          url: "Cart",
          defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
          namespaces: new[] { "GamingShop.Controllers" }
      );

            routes.MapRoute(
             name: "Add Cart",
             url: "addcart",
             defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
             namespaces: new[] { "GamingShop.Controllers" }
         );

            routes.MapRoute(
              name: "Contact",
              url: "Contact",
              defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "GamingShop.Controllers" }
          );

            routes.MapRoute(
               name: "Product Detail",
               url: "detail/{metatitle}-{Id}",
               defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
               namespaces: new[] { "GamingShop.Controllers" }
           );

            routes.MapRoute(
                name: "Product Category",
                url: "category/{metatitle}-{cateId}",
                defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "GamingShop.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "GamingShop.Controllers" }
            );
        }
    }
}