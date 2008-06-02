namespace Bindable.Linq.Samples.MessengerClient.MessengerService.Simulator
{
    using System;
    using System.Reflection;
    using Behaviors;
    using Bindable.Linq.Samples.MessengerClient.Helpers;
    using Domain;

    /// <summary>
    /// A class that generates contacts.
    /// </summary>
    internal sealed class ContactFactory : IDisposable
    {
        private static readonly string[] _femaleFirstNames = new string[] {"Angela", "Alana", "Carly", "Dawn", "Franchesca", "Gail", "Heather", "Ishita", "Jody", "Jade", "Kim", "Laura", "Natalie", "Rachel", "Sarah", "Susan", "Toni-Anne", "Wendy"};
        private static readonly string[] _lastNames = new string[] {"Adams", "Bourke", "Burela", "Brown", "Besiso", "Denny", "Davies", "Doubinski", "Evans", "Galley", "Gianone", "Hewitt", "James", "Ingram", "King", "Mitchell", "Nolan", "Pens", "Quinn", "Richards", "Stephens", "Tudhope", "Thompson", "Tusnea", "Vasios"};
        private static readonly string[] _maleFirstNames = new string[] {"Aaron", "Alan", "Adam", "Ben", "Billy", "Chad", "Chris", "Dean", "Elliot", "George", "Harry", "Ian", "Luke", "Michael", "Mike", "Owen", "Omar", "Paul", "Peter", "Richard", "Russell", "Steve", "Stephen", "Sam", "Timothy", "Thomas"};
        private readonly TimerBehavior[] _behaviors = new TimerBehavior[] {new QuoteBehavior()};
        private readonly Random _random = new Random();
        private int _lastFemalePhotoIndex = 0;
        private int _lastMalePhotoIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedContactRepository"/> class.
        /// </summary>
        public ContactFactory() {}

        #region IDisposable Members
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Creates a new contact.
        /// </summary>
        public Contact CreateContact()
        {
            var result = new Contact();
            if (_random.Next(2) == 0)
            {
                result.Name = _maleFirstNames.SelectRandom() + " " + _lastNames.SelectRandom();
                result.Photo = ExtractPhoto(false);
            }
            else
            {
                result.Name = _femaleFirstNames.SelectRandom() + " " + _lastNames.SelectRandom();
                result.Photo = ExtractPhoto(true);
            }
            result.EmailAddress = result.Name.ToLowerInvariant().Replace(" ", ".") + "@hotmail.com";
            _behaviors.SelectRandom().ApplyTo(result);
            return result;
        }

        private byte[] ExtractPhoto(bool female)
        {
            var index = 0;
            if (female)
            {
                index = ++_lastFemalePhotoIndex;
            }
            else
            {
                index = ++_lastMalePhotoIndex;
            }
            var resourceName = string.Format("{0}.ContactImages.{1}.{2}.png", typeof (ContactFactory).Namespace, (female ? "Female" : "Male"), (index % 15).ToString().PadLeft(2, '0'));

            var photo = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (photo != null)
            {
                var result = new byte[photo.Length];
                photo.Read(result, 0, (int) photo.Length);
                return result;
            }
            return null;
        }
    }
}