using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;

// General
[assembly: AssemblyTitle("Bindable.Linq")]
[assembly: AssemblyDescription("Bindable LINQ is a set of LINQ extensions that enable data binding over standard LINQ queries.")]
[assembly: AssemblyCompany("www.bindable.net/linq")]
[assembly: AssemblyProduct("Bindable LINQ")]
[assembly: AssemblyCopyright("Copyright Paul Stovell 2007-2008")]
[assembly: AssemblyCulture("")]
// Compatibility
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
// Versioning
[assembly: Guid("e38721d1-d95f-433a-ba10-9afb80c81a04")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
// Configuration
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif