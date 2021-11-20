using Caliburn.Micro;
using DADesktopUI.EventModels;
using DADesktopUI.Library;
using DADesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DADesktopUI.Library.Enums;

namespace DADesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>, IHandle<GoToEvent>
    {
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _user;
        private readonly HomeViewModel _homeVM;
        private readonly ProductsViewModel _productsVM;

        public ShellViewModel(IEventAggregator events, ILoggedInUserModel user, HomeViewModel homeVM, ProductsViewModel productsVM)
        {
            _events = events;
            _user = user;
            _homeVM = homeVM;
            _productsVM = productsVM;
            _events.SubscribeOnPublishedThread(this);
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;
                if (string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }
                return output;
            }
        }
        public bool CanHome
        {
            get
            {
                bool output = ActiveItem != _homeVM && IsLoggedIn;
                return output;
            }
        }
        public void ExitApplication()
        {
            TryCloseAsync();
        }

        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
        public async Task Home()
        {
            await ActivateItemAsync(_homeVM, new CancellationToken());
            NotifyOfPropertyChange(() => CanHome);
        }
        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_homeVM, cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
            NotifyOfPropertyChange(() => CanHome);
        }
        public async Task HandleAsync(GoToEvent message, CancellationToken cancellationToken)
        {
            Screen screenToGo;
            switch (message.View)
            {
                case GoTo.Home:
                    screenToGo = _homeVM;
                    break;
                case GoTo.Products:
                    screenToGo = _productsVM;
                    break;
                default:
                    throw new NotImplementedException("View to redirect not set.");
            }
            await ActivateItemAsync(screenToGo, cancellationToken);
            NotifyOfPropertyChange(() => CanHome);
        }
    }
}