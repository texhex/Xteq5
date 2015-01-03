using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

//SharedAssemblyInfo.cs taken from
//[Sharing assembly version across projects in a solution](http://weblogs.asp.net/ashishnjain/sharing-assembly-version-across-projects-in-a-solution) by Ashish Jain


[assembly: AssemblyTitle("TestUtil")] //Explorer: File description
[assembly: AssemblyProduct("http://www.testutil.com/")] //Explorer: Product name
[assembly: AssemblyVersion("1.23.*")] //Explorer: Product version
//[assembly: AssemblyFileVersion("1.0.0.0")] //If not set, AssemblyVersion is also used for AssemblyFileVersion. Explorer: File version


[assembly: AssemblyDescription("For more information, please visit http://www.testutil.com/")] //It's actually a comment: $File.VersionInfo.Comments
[assembly: AssemblyCopyright("Copyright © 2010-2015, Michael 'Tex' Hex")]
[assembly: AssemblyCompany("TestUtil")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCulture("")]


// Setting ComVisible to false makes the types in this assembly not visible
// to COM components. If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

