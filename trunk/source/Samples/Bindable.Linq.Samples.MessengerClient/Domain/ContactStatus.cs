namespace Bindable.Linq.Samples.MessengerClient.Domain
{
    /// <summary>
    /// Indicates the status of a contact.
    /// </summary>
    public enum ContactStatus
    {
        /// <summary>
        /// The contact is currently offline.
        /// </summary>
        Offline,
        /// <summary>
        /// The contact is currently away from the computer.
        /// </summary>
        Away,
        /// <summary>
        /// The contact is currently busy.
        /// </summary>
        Busy,
        /// <summary>
        /// The contact is currently online.
        /// </summary>
        Online
    }
}