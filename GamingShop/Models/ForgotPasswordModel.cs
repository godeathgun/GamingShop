using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamingShop.Models
{
    public class ForgotPasswordModel
    {
        [Key]
        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }
        public string Code { get; set; }
    }
}