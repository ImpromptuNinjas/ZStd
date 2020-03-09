using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using static ImpromptuNinjas.ZStd.Native.ZStdCCtx;
namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public readonly struct CCtx {

    static CCtx() => Native.Init();

    public static unsafe CCtx* Create()
      => CreateCCtx();

    public static int GetMaxCompressionLevel()
      => Native.ZStd.GetMaxCompressionLevel();

    public static int GetMinCompressionLevel()
      => Native.ZStd.GetMinCompressionLevel();

    public static ulong GetUpperBound(UIntPtr srcSize)
      => CompressBound(srcSize);

  }

  public static partial class Extensions {

    public static unsafe CCtx* Copy(ref this CCtx ctx, ulong pledgedSrcSize = ulong.MaxValue)
      => Native.ZStdCCtx.Copy((CCtx*) Unsafe.AsPointer(ref ctx), pledgedSrcSize);

    public static unsafe void Free(ref this CCtx ctx)
      => FreeCCtx((CCtx*) Unsafe.AsPointer(ref ctx));

    public static unsafe UIntPtr GetSize(ref this CCtx ctx)
      => SizeOfCCtx((CCtx*) Unsafe.AsPointer(ref ctx)).EnsureZStdSuccess();

    public static unsafe void Reset(ref this CCtx ctx, ResetDirective directive)
      => ResetCCtx((CCtx*) Unsafe.AsPointer(ref ctx), directive).EnsureZStdSuccess();

    public static unsafe int Get(ref this CCtx ctx, CompressionParameter parameter) {
      GetParameter((CCtx*) Unsafe.AsPointer(ref ctx), parameter, out var value).EnsureZStdSuccess();
      return value;
    }

    public static unsafe void Set(ref this CCtx ctx, CompressionParameter parameter, int value)
      => SetParameter((CCtx*) Unsafe.AsPointer(ref ctx), parameter, value).EnsureZStdSuccess();

    public static unsafe void UseDictionary(ref this CCtx ctx, CDict* dict)
      => ReferenceDictionary((CCtx*) Unsafe.AsPointer(ref ctx), dict).EnsureZStdSuccess();

    public static unsafe void UseDictionary(ref this CCtx ctx, ReadOnlySpan<byte> dict)
      => ReferenceDictionary((CCtx*) Unsafe.AsPointer(ref ctx), dict).EnsureZStdSuccess();

    public static unsafe void ResetDictionary(ref this CCtx ctx)
      => Native.ZStdCCtx.ResetDictionary((CCtx*) Unsafe.AsPointer(ref ctx)).EnsureZStdSuccess();

    public static unsafe UIntPtr Compress(ref this CCtx ctx, Span<byte> output, ReadOnlySpan<byte> input)
      => Native.ZStdCCtx.Compress((CCtx*) Unsafe.AsPointer(ref ctx), output, input).EnsureZStdSuccess();

    public static unsafe UIntPtr StreamCompress(ref this CCtx ctx, ref ArraySegment<byte> output, ref ArraySegment<byte> input, EndDirective endOp)
      => Native.ZStdCCtx.StreamCompress((CCtx*) Unsafe.AsPointer(ref ctx), ref output, ref input, endOp).EnsureZStdSuccess();

    public static unsafe UIntPtr StreamCompress(ref this CCtx ctx, ref Buffer output, ref Buffer input, EndDirective endOp)
      => Native.ZStdCCtx.StreamCompress((CCtx*) Unsafe.AsPointer(ref ctx), ref output, ref input, endOp).EnsureZStdSuccess();

  }

}
