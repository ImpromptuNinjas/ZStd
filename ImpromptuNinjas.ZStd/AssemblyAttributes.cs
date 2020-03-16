using System.Reflection;
using System.Runtime.CompilerServices;

#if !DEBUG

#if !NETSTANDARD1_1
[assembly: DisablePrivateReflection]
#endif

#if !NETSTANDARD1_4 && !NETSTANDARD1_1
[assembly: SuppressIldasm]
#endif

#endif

[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd.Tests.NetCore31")]
[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd.Tests.NetStandard20")]

#if NETCOREAPP3_1
[assembly: AssemblyMetadata("TFMDEF", "NETCOREAPP3.1")]
#elif NETSTANDARD2_0
[assembly: AssemblyMetadata("TFMDEF", "NETSTANDARD2_0")]
#elif NETSTANDARD1_4
[assembly: AssemblyMetadata("TFMDEF", "NETSTANDARD1_4")]
#elif NETSTANDARD1_1
[assembly: AssemblyMetadata("TFMDEF", "NETSTANDARD1_1")]
#endif
