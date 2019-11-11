using System.ComponentModel.DataAnnotations;

namespace GamingShop.Models
{
    public class RegisterModel
    {
        [Key]
        public long ID { get; set; }

        [Required(ErrorMessage ="Required")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "At least 6 key")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Wrong Confirm Password")]
        public string ConfirmPassWord { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Address { get; set; }
    }
}