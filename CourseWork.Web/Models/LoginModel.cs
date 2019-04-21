using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Web.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public enum LoginStatus
    {
        OK, WRONG_EMAIL, WRONG_PASSWORD, WRONG_EMAIL_FORMAT
    }
}
