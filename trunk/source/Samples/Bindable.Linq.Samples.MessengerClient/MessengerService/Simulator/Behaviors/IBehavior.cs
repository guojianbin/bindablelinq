namespace Bindable.Linq.Samples.MessengerClient.MessengerService.Simulator.Behaviors
{
    using System;
    using Domain;

    internal interface IBehavior : IDisposable
    {
        void ApplyTo(Contact contact);
    }
}