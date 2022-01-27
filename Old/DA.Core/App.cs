using DA.Core.Models;
using DA.Core.Services;
using DA.Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.RegisterType<ILoggedInUserModel, LoggedInUserModel>();
            //Mvx.IoCProvider.RegisterType<ILoginViewModel, LoginViewModel>();
            RegisterCustomAppStart<CustomAppStart>();
        }
    }
}
