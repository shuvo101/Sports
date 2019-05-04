using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.API.ViewModels
{
    public class LoginViewModel
    {
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class LoggedInUserInfo 
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }

    public class SuccessfulLoginResponse
    {
        public string Token { get; set; }
        public LoggedInUserInfo User { get; set; }
    }

    public class FailedLoginResponse
    {
        public int Error { get; set; }
        public string Token { get; set; } = "";
    }

    public class ErrorInformation
    {
        public string Error { get; set; }
    }

    public class LoginResponseViewModel
    {
        public SuccessfulLoginResponse successResonse { get; set; }
        public FailedLoginResponse failedResponse { get; set; }
    }
}
