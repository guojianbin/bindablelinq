using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BindingOriented.SyncLinq.Samples.MessengerClient.Domain;
using BindingOriented.SyncLinq.Samples.MessengerClient.Helpers;

namespace BindingOriented.SyncLinq.Samples.MessengerClient.MessengerService.Simulator.Behaviors
{
    /// <summary>
    /// A behavior that changes the contacts status to a boring quote every minute or so.
    /// </summary>
    internal class BoringBehavior : TimerBehavior
    {
        private string[] _quotes = new string[] {
            "Another boring day in the office.",
            "*yawn*.",
            "Looking forward to the weekend"
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="BoringBehavior"/> class.
        /// </summary>
        public BoringBehavior()
            : base(TimeSpan.FromSeconds(1))
        {

        }

        /// <summary>
        /// Triggers this instance.
        /// </summary>
        protected override void Trigger(Contact contact)
        {
            int chance = this.Random.Next(0, 70);
            if (chance == 1)
            {
                contact.TagLine = _quotes.SelectRandom();
            }
            else if (chance == 2)
            {
                contact.TagLine = "";
            }
        }
    }
}
