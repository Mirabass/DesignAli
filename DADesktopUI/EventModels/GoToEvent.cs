using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DADesktopUI.Library.Enums;

namespace DADesktopUI.EventModels
{
    public class GoToEvent
    {
        public GoTo View { get; set; }
        public GoToEvent(GoTo view)
        {
             View = view;
        }

    }
}
