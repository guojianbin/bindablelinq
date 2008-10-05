using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Bindable.Linq.Samples.Performance
{
    class Program
    {
        private const int ItemsInQuery = 100000;

        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            Console.Write("Generating test items... ");
            var contacts = new ObservableCollection<Contact>();
            for (var i = 0; i < ItemsInQuery; i++)
            {
                contacts.Add(new Contact {FirstName = "Paul " + i, LastName = "Stovell " + i}); 
            }

            Console.WriteLine("Done.");
            Test("Select",
                () => System.Linq.Enumerable.Select(contacts, c => c.FirstName),
                () => contacts.AsBindable().Select(c => c.FirstName, DependencyDiscovery.Disabled)
                );

            Test("Select Anonymous",
                () => System.Linq.Enumerable.Select(contacts, c => new { Name = c.FirstName + c.LastName }),
                () => contacts.AsBindable().Select(c => new { Name = c.FirstName + c.LastName }, DependencyDiscovery.Disabled)
                );

            Test("Where",
                () => System.Linq.Enumerable.Where(contacts, c => c.FirstName.Contains("3")),
                () => contacts.AsBindable().Where(c => c.FirstName.Contains("3"), DependencyDiscovery.Disabled)
                );

            Test("OrderBy",
                () => System.Linq.Enumerable.OrderBy(contacts, c => c.FirstName.Substring(4, 1)),
                () => contacts.AsBindable().OrderBy(c => c.FirstName.Substring(4, 1), DependencyDiscovery.Disabled)
                );

            Console.WriteLine("Complete");
            Console.ReadKey();
        }

        public class Contact
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public static void Test(string message, System.Linq.Expressions.Expression<Func<IEnumerable>> standardQuery, System.Linq.Expressions.Expression<Func<IEnumerable>> bindableQuery)
        {
            int count = 0;
            var standardCompiledQuery = standardQuery.Compile();
            var bindableCompiledQuery = bindableQuery.Compile();
            
            Trace.WriteLine(message);
            Trace.Indent();
            Trace.Write("Standard LINQ: ");
            
            var standardStopwatch = new Stopwatch();
            standardStopwatch.Start(); 
            foreach (var item in standardCompiledQuery())
            {
                count++;
            }
            if (count < 50) throw new Exception();
            count = 0;
            standardStopwatch.Stop();
            Trace.WriteLine(standardStopwatch.ElapsedMilliseconds.ToString("n0").PadLeft(15) + " ms");
            
            Trace.Write("Bindable LINQ: ");
            var bindableStopwatch = new Stopwatch();
            bindableStopwatch.Start();
            foreach (var item in bindableCompiledQuery())
            {
                count++;
            }
            if (count < 50) throw new Exception();
            bindableStopwatch.Stop();
            Trace.Write(bindableStopwatch.ElapsedMilliseconds.ToString("n0").PadLeft(15) + " ms (");
            Trace.WriteLine( (((double)bindableStopwatch.ElapsedMilliseconds / standardStopwatch.ElapsedMilliseconds ).ToString("n1")) + " times slower)");

            Trace.Unindent();
        }
    }

}
