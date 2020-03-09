using System;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public sealed class ZStdDecompressorDictionary : IDisposable {

    public unsafe DDict* Reference;

    public unsafe ZStdDecompressorDictionary(ZStdDictionaryBuilder dict)
      => Reference = DDict.Create(dict);

    private unsafe ZStdDecompressorDictionary(DDict* ctx)
      => Reference = ctx;

    public unsafe void Dispose()
      => Reference->Free();

  }

}
