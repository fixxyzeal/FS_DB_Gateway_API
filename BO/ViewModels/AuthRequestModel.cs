using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BO.ViewModels
{
    public class AuthRequestModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "AppName is required")]
        public string AppName { get; set; }

        public string AppType { get; set; }

        public string IP { get; set; }
    }
}