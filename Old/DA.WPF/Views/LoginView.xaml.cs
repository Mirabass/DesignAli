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
    //[MvxContentPresentation(WindowIdentifier = nameof(MainWindow), StackNavigation = false)]
    public partial class LoginView : MvxWpfView
    { 
        public LoginView()
        {
            InitializeComponent();
        }
    }
}
