using Caliburn.Micro;
using DADesktopUI.EventModels;
using DADesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DADesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _user;

        public ShellViewModel(IEventAggregator events, ILoggedInUserModel user)
        {
            _events = events;
            _user = user;
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

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(null, cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}