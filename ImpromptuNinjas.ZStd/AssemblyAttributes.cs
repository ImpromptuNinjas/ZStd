using System.Reflection;
using System.Runtime.CompilerServices;

#if !DEBUG
[assembly: DisablePrivateReflection]
[assembly: SuppressIldasm]
#endif

[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd.Tests.NetCore31")]
[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd.Tests.NetStandard20")]

#if NETCOREAPP3_1
[assembly: AssemblyMetadata("TFMDEF", "NETCOREAPP3.1")]
#elif NETSTANDARD2_0
[assembly: AssemblyMetadata("TFMDEF", "NETSTANDARD2_0")]
#endif
