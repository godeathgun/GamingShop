using System.ComponentModel.DataAnnotations;

namespace GamingShop.Models
{
    public class LoginModel
    {
        [Key]
        [Required(ErrorMessage = "Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }
}