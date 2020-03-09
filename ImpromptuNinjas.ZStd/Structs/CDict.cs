using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using static ImpromptuNinjas.ZStd.Native.ZStdCDict;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public readonly struct CDict {

    static CDict() => Native.Init();

    public static unsafe CDict* Create(ReadOnlySpan<byte> dict, int compressionLevel = default)
      => CreateCDict(dict, compressionLevel);

    public static unsafe CDict* Create(ZStdDictionaryBuilder dict, int compressionLevel = default)
      => CreateCDict(dict, compressionLevel);

  }

  public static partial class Extensions {

    public static unsafe void Free(ref this CDict dict)
      => FreeCDict((CDict*) Unsafe.AsPointer(ref dict));

    public static unsafe UIntPtr GetSize(ref this CDict dict)
      => SizeOfCDict((CDict*) Unsafe.AsPointer(ref dict));

  }

}
