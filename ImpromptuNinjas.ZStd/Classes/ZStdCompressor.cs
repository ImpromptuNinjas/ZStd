using System;
using JetBrains.Annotations;
using static ImpromptuNinjas.ZStd.Native.ZStdCCtx;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public class ZStdCompressor
    : IDisposable
#if !NETSTANDARD1_4 && !NETSTANDARD1_1
      , ICloneable
#endif
  {

    public unsafe CCtx* Context;

    public unsafe ZStdCompressor()
      => Context = CCtx.Create();

    private unsafe ZStdCompressor(CCtx* ctx)
      => Context = ctx;

    public unsafe void Dispose()
      => Context->Free();

    private static int? _lazyMaximumCompressionLevel;

    public static int MaximumCompressionLevel
      => _lazyMaximumCompressionLevel ??= Native.ZStd.GetMaxCompressionLevel();

    private static int? _lazyMinimumCompressionLevel;

    public static int MinimumCompressionLevel
      => _lazyMinimumCompressionLevel ??= Native.ZStd.GetMinCompressionLevel();

    public static ulong GetUpperBound(UIntPtr srcSize)
      => CompressBound(srcSize);

    public unsafe ZStdCompressor Clone(ulong pledgedSrcSize = ulong.MaxValue)
      => new ZStdCompressor(Copy(Context, pledgedSrcSize));

#if !NETSTANDARD1_4 && !NETSTANDARD1_1
    object ICloneable.Clone()
      => Clone();
#endif

    public unsafe void Free()
      => FreeCCtx(Context);

    public unsafe UIntPtr GetSize()
      => SizeOfCCtx(Context).EnsureZStdSuccess();

    public unsafe void Reset(ResetDirective directive)
      => ResetCCtx(Context, directive).EnsureZStdSuccess();

    public unsafe int Get(CompressionParameter parameter) {
      GetParameter(Context, parameter, out var value).EnsureZStdSuccess();
      return value;
    }

    public unsafe void Set(CompressionParameter parameter, int value)
      => SetParameter(Context, parameter, value).EnsureZStdSuccess();

    public unsafe void UseDictionary([CanBeNull] ZStdCompressorDictionary dict)
      => ReferenceDictionary(Context, dict != null ? dict.Reference : null).EnsureZStdSuccess();

    public unsafe void ResetDictionary()
      => Native.ZStdCCtx.ResetDictionary(Context).EnsureZStdSuccess();

    public unsafe UIntPtr Compress(Span<byte> output, ReadOnlySpan<byte> input)
      => Native.ZStdCCtx.Compress(Context, output, input).EnsureZStdSuccess();

    public unsafe UIntPtr StreamCompress(ref ArraySegment<byte> output, ref ArraySegment<byte> input, EndDirective endOp)
      => Native.ZStdCCtx.StreamCompress(Context, ref output, ref input, endOp).EnsureZStdSuccess();

    public unsafe UIntPtr StreamCompress(ref Buffer output, ref Buffer input, EndDirective endOp)
      => Native.ZStdCCtx.StreamCompress(Context, ref output, ref input, endOp).EnsureZStdSuccess();

    public unsafe UIntPtr ReadyToFlush => GetBytesReadyToFlush(Context);

  }

}
