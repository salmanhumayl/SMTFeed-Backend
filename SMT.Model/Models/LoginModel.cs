using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Model.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ForgetPasswordEmaiModel
    {

        public string EmailAddress { get; set; }

       
    }
    public class ForgetPasswordModel
    {

        public string EmailAddress { get; set; }
        
        public int UserID { get; set; }
        public string Guid { get; set; }
    }
    
    public class ForgetUserInfo
    {
        public int UserID { get; set; }
        public int id { get; set; }
    }


    public class ChangePasswordViewModel
    {
        public string NewPassword { get; set; }
        public string token { get; set; }

        public string Name { get; set; }


    }
}
