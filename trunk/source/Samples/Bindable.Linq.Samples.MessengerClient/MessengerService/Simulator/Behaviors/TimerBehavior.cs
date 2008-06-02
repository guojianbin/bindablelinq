namespace Bindable.Linq.Samples.MessengerClient.MessengerService.Simulator.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Threading;
    using Domain;

    /// <summary>
    /// A base class for contact behaviors.
    /// </summary>
    internal abstract class TimerBehavior : IBehavior
    {
        private readonly List<Contact> _contactsToVisit = new List<Contact>();
        private readonly Random _random = new Random();
        private readonly DispatcherTimer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Behavior"/> class.
        /// </summary>
        /// <param name="timespan">The timespan.</param>
        public TimerBehavior(TimeSpan timespan)
        {
            _timer = new DispatcherTimer(DispatcherPriority.SystemIdle);
            _timer.Interval = timespan;
            _timer.Tick += new EventHandler(DispatcherTimer_Trigger);
            _timer.Start();
        }

        /// <summary>
        /// Gets the contacts to visit.
        /// </summary>
        protected IEnumerable<Contact> ContactsToVisit
        {
            get { return _contactsToVisit; }
        }

        /// <summary>
        /// Gets the random.
        /// </summary>
        protected Random Random
        {
            get { return _random; }
        }

        #region IBehavior Members
        /// <summary>
        /// Applies the behavior to a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public void ApplyTo(Contact contact)
        {
            _contactsToVisit.Add(contact);
            Trigger(contact);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _timer.Stop();
        }
        #endregion

        protected abstract void Trigger(Contact contact);

        private void DispatcherTimer_Trigger(object sender, EventArgs e)
        {
            foreach (var contact in ContactsToVisit)
            {
                Trigger(contact);
            }
        }
    }
}