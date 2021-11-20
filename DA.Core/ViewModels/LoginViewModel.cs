using DA.Core.Services;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DA.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel, ILoginViewModel
    {
        private string? _userName;
        private string? _password;
        private string? _errorMessage;
        private IUserService? _userService;
        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
        }
        public override async Task Initialize()
        {
            await base.Initialize();
        }
        public string? UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
                RaisePropertyChanged(() => CanLogIn);
            }
        }

        public string? Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
                RaisePropertyChanged(() => CanLogIn);
            }
        }
        public bool CanLogIn
        {
            get
            {
                bool output = false;
                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }
                return output;
            }
        }

        public bool IsErrorVisible
        {
            get
            {
                bool output = false;
                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }
                return output;
            }
        }
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                RaisePropertyChanged(() => IsErrorVisible);
                RaisePropertyChanged(() => ErrorMessage);

            }
        }
        public async Task LogIn()
        {
            if (_userService is null) throw new Exception("User service is null");
            if (UserName is null) throw new Exception("UserName is null");
            if (Password is null) throw new Exception("Password is null");
            try
            {
                ErrorMessage = "";
                var result = await _userService.Authenticate(UserName, Password);
                if (result is null) throw new Exception("Authentication failed");
                // Capture more information about the user:
                await _userService.GetLoggedInUserInfo(result.Access_Token);

                //await _events.PublishOnUIThreadAsync(new LogOnEvent(), new CancellationToken());
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
