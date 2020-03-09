using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using static ImpromptuNinjas.ZStd.Native.ZStdDDict;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public readonly struct DDict {

    static DDict() => Native.Init();

    public static unsafe DDict* Create(ReadOnlySpan<byte> dict)
      => CreateDDict(dict);

    public static unsafe DDict* Create(ZStdDictionaryBuilder dict)
      => CreateDDict(dict);

  }

  public static partial class Extensions {

    public static unsafe void Free(ref this DDict dict)
      => FreeDDict((DDict*) Unsafe.AsPointer(ref dict));

    public static unsafe UIntPtr GetSize(ref this DDict dict)
      => SizeOfDDict((DDict*) Unsafe.AsPointer(ref dict));

  }

}
