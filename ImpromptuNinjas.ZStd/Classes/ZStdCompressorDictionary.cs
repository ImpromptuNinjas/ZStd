using System;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public sealed class ZStdCompressorDictionary : IDisposable {

    public unsafe CDict* Reference;

    public unsafe ZStdCompressorDictionary(ZStdDictionaryBuilder dict, int compressionLevel = default)
      => Reference = CDict.Create(dict, compressionLevel);

    private unsafe ZStdCompressorDictionary(CDict* ctx)
      => Reference = ctx;

    public unsafe void Dispose()
      => Reference->Free();

  }

}
