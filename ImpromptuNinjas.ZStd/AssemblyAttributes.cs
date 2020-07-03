using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using ImpromptuNinjas.ZStd;
using JetBrains.Annotations;

#if !DEBUG

#if !NETSTANDARD1_1
[assembly: DisablePrivateReflection]
#endif

#if !NETSTANDARD1_4 && !NETSTANDARD1_1
[assembly: SuppressIldasm]
#endif

#endif

#if OFFICIAL
[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd, PublicKey=" + PublicKey.Official)]
[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd.Tests.NetCore31, PublicKey=" + PublicKey.Official)]
[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd.Tests.NetStandard20, PublicKey=" + PublicKey.Official)]
#else
[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd, PublicKey=" + PublicKey.Unofficial)]
[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd.Tests.NetCore31, PublicKey=" + PublicKey.Unofficial)]
[assembly: InternalsVisibleTo("ImpromptuNinjas.ZStd.Tests.NetStandard20, PublicKey=" + PublicKey.Unofficial)]
#endif

#if NETCOREAPP3_1
[assembly: AssemblyMetadata("TFMDEF", "NETCOREAPP3.1")]
#elif NETSTANDARD2_1
[assembly: AssemblyMetadata("TFMDEF", "NETSTANDARD2_1")]
#elif NETSTANDARD2_0
[assembly: AssemblyMetadata("TFMDEF", "NETSTANDARD2_0")]
#elif NETSTANDARD1_4
[assembly: AssemblyMetadata("TFMDEF", "NETSTANDARD1_4")]
#elif NETSTANDARD1_1
[assembly: AssemblyMetadata("TFMDEF", "NETSTANDARD1_1")]
#endif

#if OFFICIAL
#if DEBUG
[assembly: AssemblyConfiguration("Official Debug")]
#else
[assembly: AssemblyConfiguration("Official Release")]
#endif
#else
#if DEBUG
[assembly: AssemblyConfiguration("Unofficial Debug")]
#else
[assembly: AssemblyConfiguration("Unofficial Release")]
#endif
#endif

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  internal static class PublicKey {

    internal const string Official =
      "0024000004800000940000000602000000240000525341310004000001000100cda836aa50e0dd" +
      "bafb6edfa693a490d192d5fc97f8422065d437fa0521c3ecc67f02382b55f6fc17e41258fb167b" +
      "a5ec97a6587cf1287f06b82f5c68e0840938bd601b4382b17caee59a045986e2bbad0aae4c4cb5" +
      "b4603fc119317a4b9ada9ad311536e1b749f05838feb782beb019f9ccbc91c83b3c36a3805f0c9" +
      "24150daa";

    internal const string Unofficial =
      "0024000004800000940000000602000000240000525341310004000001000100a1dac382057431" +
      "c40c9e53372c927b523701df386cce73cfc63986bf6511b65064e70bf6009c252da1df9ff0ec68" +
      "bc56b70f2e30d7def420f9084a94554fb2bacd2833576ad10f5610751511e2c99c71ad03bc16cb" +
      "6af1617a6ed674c386e33655100ad89c630bccd9a644e84f3545320f913d45dd5a189c15e8fda4" +
      "740da8cf";

  }

}
