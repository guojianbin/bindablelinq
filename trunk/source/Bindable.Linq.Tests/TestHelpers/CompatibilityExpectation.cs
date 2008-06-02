namespace Bindable.Linq.Tests.TestHelpers
{
    /// <summary>
    /// Represents the different compatibility levels Bindable LINQ maintains with regards to LINQ. 
    /// </summary>
    internal enum CompatibilityExpectation
    {
        /// <summary>
        /// Indicates that the order the items are stored in is important.
        /// </summary>
        FullyCompatible = 1,

        /// <summary>
        /// Indicates that it is expected that the Bindable LINQ results should contain the same items
        /// as the LINQ results, but that they may be in a different order.
        /// </summary>
        FullyCompatibleExceptOrdering = 2
    }
}