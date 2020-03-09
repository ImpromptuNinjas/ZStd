using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using static ImpromptuNinjas.ZStd.Native.ZStd;
using static ImpromptuNinjas.ZStd.Native.ZStdDCtx;
namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public readonly struct DCtx {

    static DCtx() => Native.Init();

    public static unsafe DCtx* Create()
      => CreateDCtx();

    public static ulong GetUpperBound(ReadOnlySpan<byte> src)
      => DecompressBound(src);

    public static ulong GetDecompressedSize(ReadOnlySpan<byte> src)
      => Native.ZStd.GetDecompressedSize(src);

  }

  public static partial class Extensions {

    public static unsafe DCtx* Copy(ref this DCtx ctx)
      => Native.ZStdDCtx.Copy((DCtx*) Unsafe.AsPointer(ref ctx));

    public static unsafe void Free(ref this DCtx ctx)
      => FreeDCtx((DCtx*) Unsafe.AsPointer(ref ctx));

    public static unsafe UIntPtr GetSize(ref this DCtx ctx)
      => SizeOfDCtx((DCtx*) Unsafe.AsPointer(ref ctx)).EnsureZStdSuccess();

    public static unsafe void Reset(ref this DCtx ctx, ResetDirective directive)
      => ResetDCtx((DCtx*) Unsafe.AsPointer(ref ctx), directive).EnsureZStdSuccess();

    /*
    public static unsafe int Get(ref this DCtx ctx, DecompressionParameter parameter) {
      GetParameter((DCtx*) Unsafe.AsPointer(ref ctx), parameter, out var value).EnsureZStdSuccess();
      return value;
    }
    */

    public static unsafe void Set(ref this DCtx ctx, DecompressionParameter parameter, int value)
      => SetParameter((DCtx*) Unsafe.AsPointer(ref ctx), parameter, value).EnsureZStdSuccess();

    public static unsafe void UseDictionary(ref this DCtx ctx, DDict* dict)
      => ReferenceDictionary((DCtx*) Unsafe.AsPointer(ref ctx), dict).EnsureZStdSuccess();

    public static unsafe void UseDictionary(ref this DCtx ctx, ReadOnlySpan<byte> dict)
      => ReferenceDictionary((DCtx*) Unsafe.AsPointer(ref ctx), dict).EnsureZStdSuccess();

    public static unsafe void ResetDictionary(ref this DCtx ctx)
      => Native.ZStdDCtx.ResetDictionary((DCtx*) Unsafe.AsPointer(ref ctx)).EnsureZStdSuccess();

    public static unsafe UIntPtr DecompressFrame(ref this DCtx ctx, Span<byte> output, ReadOnlySpan<byte> input)
      => Decompress((DCtx*) Unsafe.AsPointer(ref ctx), output, input).EnsureZStdSuccess();

    public static unsafe UIntPtr StreamDecompress(ref this DCtx ctx, ref ArraySegment<byte> output, ref ArraySegment<byte> input)
      => Native.ZStdDCtx.StreamDecompress((DCtx*) Unsafe.AsPointer(ref ctx), ref output, ref input).EnsureZStdSuccess();

    public static unsafe UIntPtr StreamDecompress(ref this DCtx ctx, ref Buffer output, ref Buffer input)
      => Native.ZStdDCtx.StreamDecompress((DCtx*) Unsafe.AsPointer(ref ctx), ref output, ref input).EnsureZStdSuccess();

  }

}
