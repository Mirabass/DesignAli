using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Core.Services
{
    public class LoginService : ILoginService
    {
        private bool _isLoggedIn;

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; }
        }

    }
}
