using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BO.ViewModels
{
    public class LogViewModel
    {
        [Required(ErrorMessage = "AppName is required")]
        public string AppName { get; set; }

        [Required(ErrorMessage = "AppType is required")]
        public string AppType { get; set; }

        public string IP { get; set; }

        public string User { get; set; }

        public string Detail { get; set; }
    }
}