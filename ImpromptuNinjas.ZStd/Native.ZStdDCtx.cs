using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  public static unsafe partial class Native {

    [PublicAPI]
    public static class ZStdDCtx {

      static ZStdDCtx()
        => Init();

      #region Dynamic Library Import Table

      // ReSharper disable IdentifierTypo
      // ReSharper disable StringLiteralTypo
      // ReSharper disable InconsistentNaming

      //private static readonly IntPtr ZSTD_DCtx_getParameter = NativeLibrary.GetExport(LoadedLib, nameof(ZSTD_DCtx_getParameter));

      private static readonly IntPtr ZSTD_DCtx_loadDictionary_byReference = NativeLibrary.GetExport(Lib, nameof(ZSTD_DCtx_loadDictionary_byReference));

      private static readonly IntPtr ZSTD_DCtx_refDDict = NativeLibrary.GetExport(Lib, nameof(ZSTD_DCtx_refDDict));

      private static readonly IntPtr ZSTD_DCtx_reset = NativeLibrary.GetExport(Lib, nameof(ZSTD_DCtx_reset));

      private static readonly IntPtr ZSTD_DCtx_setParameter = NativeLibrary.GetExport(Lib, nameof(ZSTD_DCtx_setParameter));

      private static readonly IntPtr ZSTD_copyDCtx = NativeLibrary.GetExport(Lib, nameof(ZSTD_copyDCtx));

      private static readonly IntPtr ZSTD_createDCtx = NativeLibrary.GetExport(Lib, nameof(ZSTD_createDCtx));

      private static readonly IntPtr ZSTD_dParam_getBounds = NativeLibrary.GetExport(Lib, nameof(ZSTD_dParam_getBounds));

      private static readonly IntPtr ZSTD_decompressDCtx = NativeLibrary.GetExport(Lib, nameof(ZSTD_decompressDCtx));

      private static readonly IntPtr ZSTD_decompressStream = NativeLibrary.GetExport(Lib, nameof(ZSTD_decompressStream));

      private static readonly IntPtr ZSTD_freeDCtx = NativeLibrary.GetExport(Lib, nameof(ZSTD_freeDCtx));

      private static readonly IntPtr ZSTD_sizeof_DCtx = NativeLibrary.GetExport(Lib, nameof(ZSTD_sizeof_DCtx));

      // ReSharper restore InconsistentNaming
      // ReSharper restore StringLiteralTypo
      // ReSharper restore IdentifierTypo

      #endregion

      /*
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr GetParameter(DCtx* dCtx, DecompressionParameter parameter, out int value) {
        value = default;
        Push(dCtx);
        Push(parameter);
        Push(ref value);
        Push(ZSTD_DCtx_getParameter);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(DecompressionParameter), typeof(int).MakeByRefType()));
        return Return<UIntPtr>();
      }
      */

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr ReferenceDictionary(DCtx* dCtx, DDict* dDict) {
        IL.Push(dCtx);
        IL.Push(dDict);
        IL.Push(ZSTD_DCtx_refDDict);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(DCtx*), typeof(DDict*)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr ResetDictionary(DCtx* cDtx)
        => ReferenceDictionary(cDtx, default(DDict*));

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr ReferenceDictionary(DCtx* dCtx, byte* dict, UIntPtr dictSize) {
        IL.Push(dCtx);
        IL.Push(dict);
        IL.Push(dictSize);
        IL.Push(ZSTD_DCtx_loadDictionary_byReference);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(DCtx*), typeof(byte*), typeof(UIntPtr)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr ReferenceDictionary(DCtx* cCtx, ReadOnlySpan<byte> dict) {
        fixed (byte* pDict = dict)
          return ReferenceDictionary(cCtx, pDict, (UIntPtr) dict.Length);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static DCtx* CreateDCtx() {
        IL.Push(ZSTD_createDCtx);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(DCtx*)));
        return IL.ReturnPointer<DCtx>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr FreeDCtx(DCtx* dCtx) {
        IL.Push(dCtx);
        IL.Push(ZSTD_freeDCtx);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(DCtx*)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr SizeOfDCtx(DCtx* dCtx) {
        IL.Push(dCtx);
        IL.Push(ZSTD_sizeof_DCtx);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(DCtx*)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr ResetDCtx(DCtx* dCtx, ResetDirective reset) {
        IL.Push(dCtx);
        IL.Push(reset);
        IL.Push(ZSTD_DCtx_reset);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(DCtx*), typeof(ResetDirective)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr Decompress(DCtx* ctx, byte* dst, UIntPtr dstCapacity, byte* src, UIntPtr srcSize) {
        IL.Push(ctx);
        IL.Push(dst);
        IL.Push(dstCapacity);
        IL.Push(src);
        IL.Push(srcSize);
        IL.Push(ZSTD_decompressDCtx);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(DCtx*), typeof(byte*), typeof(UIntPtr), typeof(byte*), typeof(UIntPtr)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr Decompress(DCtx* ctx, Span<byte> dst, ReadOnlySpan<byte> src) {
        fixed (byte* pDst = dst)
        fixed (byte* pSrc = src)
          return Decompress(ctx, pDst, (UIntPtr) dst.Length, pSrc, (UIntPtr) src.Length);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr SetParameter(DCtx* dCtx, DecompressionParameter parameter, int value) {
        IL.Push(dCtx);
        IL.Push(parameter);
        IL.Push(value);
        IL.Push(ZSTD_DCtx_setParameter);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(DecompressionParameter), typeof(int)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ParameterBounds GetParameterBounds(DecompressionParameter dParam) {
        IL.Push(dParam);
        IL.Push(ZSTD_dParam_getBounds);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(ParameterBounds),
          typeof(DecompressionParameter)));
        return IL.Return<ParameterBounds>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr StreamDecompress(DCtx* ctx, ref Buffer output, ref Buffer input) {
        IL.Push(ctx);
        IL.Push(ref output);
        IL.Push(ref input);
        IL.Push(ZSTD_decompressStream);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(DCtx).MakeByRefType(), typeof(Buffer).MakeByRefType(), typeof(Buffer).MakeByRefType()));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr StreamDecompress(DCtx* ctx, ref ArraySegment<byte> output, ref ArraySegment<byte> input) {
        UIntPtr result = default;

        Buffer.WithArraySegmentPair(ref output, ref input, (ref Buffer outBuf, ref Buffer inBuf) => {
          result = StreamDecompress(ctx, ref outBuf, ref inBuf);
        });

        return result;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr Copy(DCtx** dCtx, DCtx* preparedDCtx) {
        IL.Push(dCtx);
        IL.Push(preparedDCtx);
        IL.Push(ZSTD_copyDCtx);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(DCtx**), typeof(Buffer).MakeByRefType(), typeof(Buffer).MakeByRefType()));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static DCtx* Copy(DCtx* ctx) {
        DCtx* newCtx;
        Copy(&newCtx, ctx).EnsureZStdSuccess();
        return newCtx;
      }

    }

  }

}
