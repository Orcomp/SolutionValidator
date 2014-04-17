using System.Reflection;
using System.Runtime.InteropServices;
using CommandLine;

[assembly: AssemblyUsage(
	" ",
	"Usage: SolutionValidator",
	"       SolutionValidator -v -rc:\\myreporoot\\ -cc:\\myconfig.config"
	)]

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("SolutionValidator")]
[assembly: AssemblyDescription("SolutionValidator Console")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("354a7c7b-9219-47c0-88f5-eb370ead245a")]

// For version information see the centralized SolutionInfo.cs
// Do not comment out the following lines:
//[assembly: AssemblyVersion("1.0.0.0")]
//[assembly: AssemblyFileVersion("1.0.0.0")]
//[assembly: AssemblyInformationalVersion("1.0.0.0")]