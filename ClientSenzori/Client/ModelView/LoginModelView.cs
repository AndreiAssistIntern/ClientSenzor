using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSenzori.Client.ModelView
{
    public class LoginModelView
    {

        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is requiered")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MaxLength(50)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}