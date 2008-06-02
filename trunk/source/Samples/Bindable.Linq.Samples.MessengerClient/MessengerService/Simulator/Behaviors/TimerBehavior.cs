using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Bindable.Linq.Samples.MessengerClient.Domain;

namespace Bindable.Linq.Samples.MessengerClient.MessengerService.Simulator.Behaviors
{
    /// <summary>
    /// A base class for contact behaviors.
    /// </summary>
    internal abstract class TimerBehavior : IBehavior
    {
        private readonly List<Contact> _contactsToVisit = new List<Contact>();
        private readonly DispatcherTimer _timer;
        private readonly Random _random = new Random();

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
            get
            {
                return _contactsToVisit;
            }
        }

        /// <summary>
        /// Gets the random.
        /// </summary>
        protected Random Random
        {
            get
            {
                return _random;
            }
        }

        protected abstract void Trigger(Contact contact);

        /// <summary>
        /// Applies the behavior to a contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public void ApplyTo(Contact contact)
        {
            _contactsToVisit.Add(contact);
            Trigger(contact);
        }

        private void DispatcherTimer_Trigger(object sender, EventArgs e)
        {
            foreach (Contact contact in this.ContactsToVisit)
            {
                this.Trigger(contact);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _timer.Stop();
        }
    }
}
