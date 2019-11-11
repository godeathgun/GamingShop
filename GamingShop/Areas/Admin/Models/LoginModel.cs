using System;
using System.ComponentModel.DataAnnotations;

namespace GamingShop.Areas.Admin.Models
{
    [Serializable]
    public class LoginModel
    {
        [Required(ErrorMessage = "Mời nhập user name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mời nhập password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}