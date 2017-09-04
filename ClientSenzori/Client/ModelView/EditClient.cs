using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSenzori.Client.ModelView
{
    public class EditClient
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The current password is requiered")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        [MaxLength(50)]
        [MinLength(8)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Password is requiered")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MaxLength(50)]
        [MinLength(8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password required")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password")]
        public string ConfirmPassowrd { get; set; }

        public HttpPostedFileBase file { get; set; }

        public string ImagePath { get; set; }
    }
}