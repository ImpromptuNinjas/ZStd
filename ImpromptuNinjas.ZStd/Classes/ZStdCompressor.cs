using System;
using JetBrains.Annotations;
using static ImpromptuNinjas.ZStd.Native.ZStdCCtx;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public class ZStdCompressor
    : IDisposable
#if !NETSTANDARD1_4 && !NETSTANDARD1_1
      ,
      ICloneable
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

    public static UIntPtr GetUpperBound(UIntPtr srcSize)
      => CompressBound(srcSize);

    public unsafe ZStdCompressor Clone(ulong pledgedSrcSize = ulong.MaxValue)
      => new ZStdCompressor(Copy(Context, pledgedSrcSize));

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

    public unsafe int Get(CompressionParameter parameter)
      => Context->Get(parameter);

    public unsafe void Set(CompressionParameter parameter, int value)
      => Context->Set(parameter, value);

    public unsafe void UseDictionary([CanBeNull] ZStdCompressorDictionary dict)
      => Context->UseDictionary(dict != null ? dict.Reference : null);

    public unsafe void ResetDictionary()
      => Context->ResetDictionary();

    public unsafe UIntPtr Compress(Span<byte> output, ReadOnlySpan<byte> input)
      => Context->Compress(output, input);

    public unsafe UIntPtr StreamCompress(ref ArraySegment<byte> output, ref ArraySegment<byte> input, EndDirective endOp)
      => Context->StreamCompress(ref output, ref input, endOp);

    public unsafe UIntPtr StreamCompress(ref Buffer output, ref Buffer input, EndDirective endOp)
      => Context->StreamCompress( ref output, ref input, endOp);

    public unsafe UIntPtr ReadyToFlush => Context->GetBytesReadyToFlush();

  }

}
