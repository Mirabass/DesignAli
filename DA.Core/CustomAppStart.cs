using DA.Core.Services;
using DA.Core.ViewModels;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DA.Core
{
    public class CustomAppStart : MvxAppStart
    {
        ILoginService _loginService;
        public CustomAppStart(IMvxApplication application,
            IMvxNavigationService navigationService,
            ILoginService loginService)
            : base(application, navigationService)
        {
            _loginService = loginService;
        }

        protected override async Task NavigateToFirstViewModel(object? hint = null)
        {
            if (_loginService.IsLoggedIn)
            {
                //NavigationService.Navigate<>
                throw new NotImplementedException();
            }
            else
            {
                await NavigationService.Navigate<LoginViewModel>();
            }
        }
        
    }
}
