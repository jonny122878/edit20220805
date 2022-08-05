using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.ViewModels
{
    public class LoginViewModel
    {
        //[Required]
        //[EmailAddress]
        public string Email { get; set; }

        //[Required]
        //[DataType(DataType.Text)]
        //public string Username { get; set; }

        public string MAC_IP{get;set;}

        //[Required]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
