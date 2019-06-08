using System;
using System.ComponentModel.DataAnnotations;

namespace Galaxy.Web.Models.Accounts
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(6, ErrorMessage ="Password must be at least 6 characters long")]
        public string Password {get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
