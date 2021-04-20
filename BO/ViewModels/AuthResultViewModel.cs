using System;
using System.Collections.Generic;
using System.Text;

namespace BO.ViewModels
{
    public class AuthResultViewModel
    {
        public string Access_Token { get; set; }
        public int Expires_In { get; set; }
    }
}