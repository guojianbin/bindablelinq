using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace Bindable.Linq.Tests.TestHelpers
{
    /// <summary>
    /// A helper class to compare Bindable LINQ queries with their LINQ to Objects counterpart.
    /// </summary>
    internal static class CompatibilityValidator
    {
        /// <summary>
        /// Compares the Bindable LINQ query with the equivalent LINQ query.
        /// </summary>
        /// <param name="syncLinqQuery">The Bindable LINQ query.</param>
        /// <param name="linqQuery">The LINQ query.</param>
        public static void CompareWithLinq(CompatibilityExpectation expectations, IEnumerable syncLinqQuery, IEnumerable linqQuery)
        {
            switch (expectations)
            {
                case CompatibilityExpectation.FullyCompatible:
                    CompareWithLinqOrdered(syncLinqQuery, linqQuery);
                    break;
                case CompatibilityExpectation.FullyCompatibleExceptOrdering:
                    CompareWithLinqUnordered(syncLinqQuery, linqQuery);
                    break;
            }
        }

        /// <summary>
        /// Compares a Bindable LINQ query with a LINQ query.
        /// </summary>
        /// <param name="syncLinqCollection">The sync linq collection.</param>
        /// <param name="linqQuery">The linq query.</param>
        private static void CompareWithLinqOrdered(IEnumerable syncLinqCollection, IEnumerable linqQuery)
        {
            CompareOrdered(syncLinqCollection, linqQuery);
        }

        /// <summary>
        /// Compares a Bindable LINQ query with a LINQ query.
        /// </summary>
        /// <param name="syncLinqCollection">The sync linq collection.</param>
        /// <param name="linqQuery">The linq query.</param>
        private static void CompareWithLinqUnordered(IEnumerable syncLinqCollection, IEnumerable linqQuery)
        {
            if (!CompareUnordered(syncLinqCollection, linqQuery))
            {
                Assert.Fail(string.Format("Iterator {0} does not match Iterator {1}.", syncLinqCollection, linqQuery));
            }
        }

        private static void CompareOrdered(IEnumerable left, IEnumerable right)
        {
            IEnumerator leftEnumerator = left.GetEnumerator();
            IEnumerator rightEnumerator = right.GetEnumerator();
            int index = 0;
            while (leftEnumerator.MoveNext() | rightEnumerator.MoveNext())
            {
                if (leftEnumerator.Current is IEnumerable)
                {
                    IEnumerable leftChildIterator = leftEnumerator.Current as IEnumerable;
                    IEnumerable rightChildIterator = rightEnumerator.Current as IEnumerable;
                    if (leftChildIterator != null && rightChildIterator != null)
                    {
                        CompareOrdered(leftChildIterator, rightChildIterator);
                    }
                }
                else if (!CompareObject(leftEnumerator.Current, rightEnumerator.Current))
                {
                    Assert.Fail(string.Format("Error when comparing Iterator '{0}' with Iterator '{1}': Items at index {2} ('{3}' : '{4}') do not match.",
                        left.ToString(), right.ToString(), index, leftEnumerator.Current, rightEnumerator.Current
                        ));
                }
                index++;
            }
        }

        private static bool CompareUnordered(IEnumerable left, IEnumerable right)
        {
            bool equal = false;
            ArrayList leftList = new ArrayList();
            ArrayList rightList = new ArrayList();
            foreach (object o in left)
            {
                leftList.Add(o);
            }
            foreach (object o in right)
            {
                rightList.Add(o);
            }

            if (leftList.Count == rightList.Count)
            {
                equal = true;
                foreach (object leftItem in leftList)
                {
                    bool leftItemFound = false;
                    if (leftItem is IEnumerable)
                    {
                        foreach (object rightItem in rightList)
                        {
                            if (CompareUnordered(leftItem as IEnumerable, rightItem as IEnumerable))
                            {
                                leftItemFound = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        object rightItem = FindEqual(leftItem, rightList);
                        if (rightItem != null && ContainsEqual(leftItem, rightList))
                        {
                            leftItemFound = true;
                        }
                    }
                    if (!leftItemFound)
                    {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
        }

        private static object FindEqual(object find, IEnumerable list)
        {
            object result = null;
            foreach (object item in list)
            {
                if (CompareObject(find, item))
                {
                    result = item;
                    break;
                }
            }
            return result;
        }

        private static bool ContainsEqual(object find, IEnumerable list)
        {
            return FindEqual(find, list) != null;
        }

        private static bool CompareObject(object left, object right)
        {
            bool equal = true;
            if (left != right)
            {
                foreach (PropertyInfo leftProperty in left.GetType().GetProperties())
                {
                    object leftValue = leftProperty.GetValue(left, null);
                    object rightValue = right.GetType().GetProperty(leftProperty.Name).GetValue(right, null);
                    if (!((leftValue == null && rightValue == null) || leftValue.Equals(rightValue)))
                    {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
        }
    }
}
