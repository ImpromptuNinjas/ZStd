using System;
using JetBrains.Annotations;
using static ImpromptuNinjas.ZStd.Native.ZStd;
using static ImpromptuNinjas.ZStd.Native.ZStdDCtx;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public class ZStdDecompressor
    : IDisposable
#if !NETSTANDARD1_4 && !NETSTANDARD1_1
      ,
      ICloneable
#endif
  {

    public unsafe DCtx* Context;

    public unsafe ZStdDecompressor()
      => Context = DCtx.Create();

    private unsafe ZStdDecompressor(DCtx* ctx)
      => Context = ctx;

    public unsafe void Dispose()
      => Context->Free();

    public static ulong GetUpperBound(ReadOnlySpan<byte> src)
      => DecompressBound(src);

    public static ulong GetDecompressedSize(ReadOnlySpan<byte> src)
      => Native.ZStd.GetDecompressedSize(src);

    public unsafe ZStdDecompressor Clone()
      => new ZStdDecompressor(Copy(Context));

#if !NETSTANDARD1_4 && !NETSTANDARD1_1
    object ICloneable.Clone()
      => Clone();
#endif

    public unsafe void Free()
      => Context->Free();

    public unsafe UIntPtr GetSize()
      => Context->GetSize();

    public unsafe void Reset(ResetDirective directive)
      => Context->Reset(directive);

    /*
    public unsafe int Get(DecompressionParameter parameter)
      => Context->Get(parameter, out var value);
    */

    public unsafe void Set(DecompressionParameter parameter, int value)
      => Context->Set(parameter, value);

    public unsafe void UseDictionary([CanBeNull] ZStdDecompressorDictionary dict)
      => Context->UseDictionary(dict != null ? dict.Reference : null);

    public unsafe void ResetDictionary()
      => Context->ResetDictionary();

    public unsafe UIntPtr Decompress(Span<byte> output, ReadOnlySpan<byte> input)
      => Context->DecompressFrame(output, input);

    public unsafe UIntPtr StreamDecompress(ref ArraySegment<byte> output, ref ArraySegment<byte> input)
      => Context->StreamDecompress(ref output, ref input);

    public unsafe UIntPtr StreamDecompress(ref Buffer output, ref Buffer input)
      => Context->StreamDecompress(ref output, ref input);

  }

}
