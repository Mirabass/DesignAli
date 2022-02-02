using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Core.ViewModels
{
    public class MainViewModel : MvxNavigationViewModel
    {
        public MainViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService) : base(logFactory, navigationService)
        {
            // navigate to nested view
            ShowNestedViewModelCommand = new MvxAsyncCommand(() => NavigationService.Navigate<LoginViewModel>());

            ShowNestedViewModelCommand.Execute();
        }
        public IMvxAsyncCommand ShowNestedViewModelCommand { get; protected set; }
    }
}
