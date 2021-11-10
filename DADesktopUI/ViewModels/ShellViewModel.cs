using Caliburn.Micro;
using DADesktopUI.EventModels;
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
        private readonly SimpleContainer _container;

        public ShellViewModel(LoginViewModel loginVM, IEventAggregator events, SimpleContainer container)
        {
            _events = events;
            _container = container;
            _events.SubscribeOnPublishedThread(this);
            ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(null, cancellationToken);
            //NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}