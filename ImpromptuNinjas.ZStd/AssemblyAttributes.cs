using System.Runtime.CompilerServices;

#if !DEBUG
[assembly: DisablePrivateReflection]
[assembly: SuppressIldasm]
[assembly: Debuggable(false,false)]
#else
[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd.Tests")]
#endif
