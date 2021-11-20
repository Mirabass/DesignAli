using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DADesktopUI.ViewModels
{
    public class StatusInfoViewModel : Screen
    {
        private readonly IWindowManager _window;
        private string _title;

        public StatusInfoViewModel(IWindowManager window)
        {
            _window = window;
        }

        public string Header { get; private set; }
        public string Message { get; private set; }

        public void UpdateMessage(string title, string header, string message)
        {
            Header = header;
            Message = message;
            _title = title;
            NotifyOfPropertyChange(() => Header);
            NotifyOfPropertyChange(() => Message);
        }
        public async Task CloseAsync()
        {
            await TryCloseAsync();
        }
        public async Task ShowDialogAsync()
        {
            dynamic settings = new ExpandoObject();
            settings.WindowtartupLocation = WindowStartupLocation.CenterOwner;
            settings.ResizeMode = ResizeMode.NoResize;
            await _window.ShowWindowAsync(this, null, settings);
        }
    }
}
