using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Samples.MessengerClient.Domain;

namespace Bindable.Linq.Samples.MessengerClient.MessengerService.Simulator.Behaviors
{
    internal interface IBehavior : IDisposable
    {
        void ApplyTo(Contact contact);
    }
}
