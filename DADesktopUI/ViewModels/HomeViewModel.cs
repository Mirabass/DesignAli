using Caliburn.Micro;
using DADesktopUI.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DADesktopUI.Library.Enums;

namespace DADesktopUI.ViewModels
{
    public class HomeViewModel: Screen
    {
        IEventAggregator _events;
        public HomeViewModel(IEventAggregator events)
        {
            _events = events;
        }

        public async Task GoToProductList()
        {
            await _events.PublishOnUIThreadAsync(new GoToEvent(GoTo.Products), new CancellationToken());
        }
    }
}
