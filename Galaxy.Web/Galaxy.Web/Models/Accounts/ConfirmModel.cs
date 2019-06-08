using System;
using System.ComponentModel.DataAnnotations;

namespace Galaxy.Web.Models.Accounts
{
    public class ConfirmModel
    {
        [Required(ErrorMessage ="Email is required")]
        [Display(Name ="Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="Code is Required")]
        public string Code { get; set; }
    }
}
