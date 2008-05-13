using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Samples.MessengerClient.Domain;
using Bindable.Linq.Samples.MessengerClient.Helpers;

namespace Bindable.Linq.Samples.MessengerClient.MessengerService.Simulator.Behaviors
{
    /// <summary>
    /// A behavior that changes the contacts status to a boring quote every minute or so.
    /// </summary>
    internal class QuoteBehavior : TimerBehavior
    {
        private string[] _quotes = new string[] {
            "Another boring day in the office.",
            "*yawn*.",
            "Looking forward to the weekend",
            "@Docklands",
            "I'm a little teapot...",
            "Windows Server 2008!",
            "BRB, pizza is here",
            "supakalafragilisticexpialidosius",
            "in Melbourne",
            "What can: can; what no can: no can!"
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="QuoteBehavior"/> class.
        /// </summary>
        public QuoteBehavior()
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
