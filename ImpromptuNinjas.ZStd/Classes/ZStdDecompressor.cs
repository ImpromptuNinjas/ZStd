using System;
using JetBrains.Annotations;
using static ImpromptuNinjas.ZStd.Native.ZStd;
using static ImpromptuNinjas.ZStd.Native.ZStdDCtx;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public class ZStdDecompressor : IDisposable, ICloneable {

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

    object ICloneable.Clone()
      => Clone();

    public unsafe void Free()
      => FreeDCtx(Context);

    public unsafe UIntPtr GetSize()
      => SizeOfDCtx(Context).EnsureZStdSuccess();

    public unsafe void Reset(ResetDirective directive)
      => ResetDCtx(Context, directive).EnsureZStdSuccess();

    /*
    public unsafe int Get(DecompressionParameter parameter) {
      GetParameter(Context, parameter, out var value).EnsureZStdSuccess();
      return value;
    }
    */

    public unsafe void Set(DecompressionParameter parameter, int value)
      => SetParameter(Context, parameter, value).EnsureZStdSuccess();

    public unsafe void UseDictionary([CanBeNull] ZStdDecompressorDictionary dict)
      => ReferenceDictionary(Context, dict != null ? dict.Reference : null).EnsureZStdSuccess();

    public unsafe void ResetDictionary()
      => Native.ZStdDCtx.ResetDictionary(Context).EnsureZStdSuccess();

    public unsafe UIntPtr Decompress(Span<byte> output, ReadOnlySpan<byte> input)
      => Native.ZStdDCtx.Decompress(Context, output, input).EnsureZStdSuccess();

    public unsafe UIntPtr StreamDecompress(ref ArraySegment<byte> output, ref ArraySegment<byte> input)
      => Native.ZStdDCtx.StreamDecompress(Context, ref output, ref input).EnsureZStdSuccess();

    public unsafe UIntPtr StreamDecompress(ref Buffer output, ref Buffer input)
      => Native.ZStdDCtx.StreamDecompress(Context, ref output, ref input).EnsureZStdSuccess();

  }

}
