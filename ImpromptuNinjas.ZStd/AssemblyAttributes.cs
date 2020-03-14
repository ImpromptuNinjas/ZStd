using System.Reflection;
using System.Runtime.CompilerServices;

#if !DEBUG
[assembly: DisablePrivateReflection]
[assembly: SuppressIldasm]
#else
[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd.Tests")]
#endif

#if NETCOREAPP3_1
[assembly: AssemblyMetadata("TFMDEF", "NETCOREAPP3.1")]
#elif NETSTANDARD2_0
[assembly: AssemblyMetadata("TFMDEF", "NETSTANDARD2_0")]
#endif
