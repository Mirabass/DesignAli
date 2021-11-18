using DA.Core.ViewModels;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using Mvx.Wpf.ItemsPresenter;
using System;


namespace DA.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    //[MvxWpfPresenter("MainWindowRegion")]
    public partial class LoginView : MvxWpfView//<LoginViewModel>
    { 
        public LoginView()
        {
            InitializeComponent();
        }
    }
}
