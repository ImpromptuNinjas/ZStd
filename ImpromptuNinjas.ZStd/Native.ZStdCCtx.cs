using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;
using JetBrains.Annotations;
namespace ImpromptuNinjas.ZStd {

  public static unsafe partial class Native {

    [PublicAPI]
    public static class ZStdCCtx {

      static ZStdCCtx()
        => Init();

      #region Dynamic Library Import Table

      // ReSharper disable IdentifierTypo
      // ReSharper disable StringLiteralTypo
      // ReSharper disable InconsistentNaming

      private static readonly IntPtr ZSTD_CCtx_getParameter = NativeLibrary.GetExport(Lib, nameof(ZSTD_CCtx_getParameter));

      private static readonly IntPtr ZSTD_CCtx_loadDictionary_byReference = NativeLibrary.GetExport(Lib, nameof(ZSTD_CCtx_loadDictionary_byReference));

      private static readonly IntPtr ZSTD_CCtx_refCDict = NativeLibrary.GetExport(Lib, nameof(ZSTD_CCtx_refCDict));

      private static readonly IntPtr ZSTD_CCtx_reset = NativeLibrary.GetExport(Lib, nameof(ZSTD_CCtx_reset));

      private static readonly IntPtr ZSTD_CCtx_setParameter = NativeLibrary.GetExport(Lib, nameof(ZSTD_CCtx_setParameter));

      private static readonly IntPtr ZSTD_CCtx_setPledgedSrcSize = NativeLibrary.GetExport(Lib, nameof(ZSTD_CCtx_setPledgedSrcSize));

      private static readonly IntPtr ZSTD_cParam_getBounds = NativeLibrary.GetExport(Lib, nameof(ZSTD_cParam_getBounds));

      private static readonly IntPtr ZSTD_compress2 = NativeLibrary.GetExport(Lib, nameof(ZSTD_compress2));

      private static readonly IntPtr ZSTD_compressBound = NativeLibrary.GetExport(Lib, nameof(ZSTD_compressBound));

      private static readonly IntPtr ZSTD_compressStream2 = NativeLibrary.GetExport(Lib, nameof(ZSTD_compressStream2));

      private static readonly IntPtr ZSTD_compressStream2_simpleArgs = NativeLibrary.GetExport(Lib, nameof(ZSTD_compressStream2_simpleArgs));

      private static readonly IntPtr ZSTD_copyCCtx = NativeLibrary.GetExport(Lib, nameof(ZSTD_copyCCtx));

      private static readonly IntPtr ZSTD_createCCtx = NativeLibrary.GetExport(Lib, nameof(ZSTD_createCCtx));

      private static readonly IntPtr ZSTD_freeCCtx = NativeLibrary.GetExport(Lib, nameof(ZSTD_freeCCtx));

      private static readonly IntPtr ZSTD_sizeof_CCtx = NativeLibrary.GetExport(Lib, nameof(ZSTD_sizeof_CCtx));

      private static readonly IntPtr ZSTD_toFlushNow = NativeLibrary.GetExport(Lib, nameof(ZSTD_toFlushNow));

      // ReSharper restore InconsistentNaming
      // ReSharper restore StringLiteralTypo
      // ReSharper restore IdentifierTypo

      #endregion

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr ReferenceDictionary(CCtx* cCtx, CDict* cDict) {
        IL.Push(cCtx);
        IL.Push(cDict);
        IL.Push(ZSTD_CCtx_refCDict);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(CDict*)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr ResetDictionary(CCtx* cCtx)
        => ReferenceDictionary(cCtx, default(CDict*));

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr ReferenceDictionary(CCtx* cCtx, byte* dict, UIntPtr dictSize) {
        IL.Push(cCtx);
        IL.Push(dict);
        IL.Push(dictSize);
        IL.Push(ZSTD_CCtx_loadDictionary_byReference);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(byte*), typeof(UIntPtr)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr ReferenceDictionary(CCtx* cCtx, ReadOnlySpan<byte> dict) {
        fixed (byte* pDict = dict)
          return ReferenceDictionary(cCtx, pDict, (UIntPtr) dict.Length);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static CCtx* CreateCCtx() {
        IL.Push(ZSTD_createCCtx);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(CCtx*)));
        return IL.ReturnPointer<CCtx>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr FreeCCtx(CCtx* cCtx) {
        IL.Push(cCtx);
        IL.Push(ZSTD_freeCCtx);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr CompressBound(UIntPtr srcSize) {
        IL.Push(srcSize);
        IL.Push(ZSTD_compressBound);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(UIntPtr)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr SizeOfCCtx(CCtx* cCtx) {
        IL.Push(cCtx);
        IL.Push(ZSTD_sizeof_CCtx);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr ResetCCtx(CCtx* cCtx, ResetDirective reset) {
        IL.Push(cCtx);
        IL.Push(reset);
        IL.Push(ZSTD_CCtx_reset);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(ResetDirective)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr Compress(CCtx* ctx, byte* dst, UIntPtr dstCapacity, byte* src, UIntPtr srcSize) {
        IL.Push(ctx);
        IL.Push(dst);
        IL.Push(dstCapacity);
        IL.Push(src);
        IL.Push(srcSize);
        IL.Push(ZSTD_compress2);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(byte*), typeof(UIntPtr), typeof(byte*), typeof(UIntPtr)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr Compress(CCtx* ctx, Span<byte> dst, ReadOnlySpan<byte> src) {
        fixed (byte* pDst = dst)
        fixed (byte* pSrc = src)
          return Compress(ctx, pDst, (UIntPtr) dst.Length, pSrc, (UIntPtr) src.Length);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr SetParameter(CCtx* cCtx, CompressionParameter parameter, int value) {
        IL.Push(cCtx);
        IL.Push(parameter);
        IL.Push(value);
        IL.Push(ZSTD_CCtx_setParameter);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(CompressionParameter), typeof(int)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr GetParameter(CCtx* cCtx, CompressionParameter parameter, int* value) {
        IL.Push(cCtx);
        IL.Push(parameter);
        IL.Push(value);
        IL.Push(ZSTD_CCtx_getParameter);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(CompressionParameter), typeof(int*)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr GetParameter(CCtx* cCtx, CompressionParameter parameter, out int value) {
        value = default;
        fixed (int* pValue = &value)
          return GetParameter(cCtx, parameter, pValue);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ParameterBounds GetParameterBounds(CompressionParameter cParam) {
        IL.Push(cParam);
        IL.Push(ZSTD_cParam_getBounds);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(ParameterBounds),
          typeof(DecompressionParameter)));
        return IL.Return<ParameterBounds>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr GetParameterBounds(CCtx* cCtx, ulong pledgedSrcSize) {
        IL.Push(cCtx);
        IL.Push(pledgedSrcSize);
        IL.Push(ZSTD_CCtx_setPledgedSrcSize);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(ulong)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr StreamCompress(CCtx* ctx, Buffer* output, Buffer* input, EndDirective endOp) {
        IL.Push(ctx);
        IL.Push(output);
        IL.Push(input);
        IL.Push(endOp);
        IL.Push(ZSTD_compressStream2);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(Buffer*), typeof(Buffer*), typeof(EndDirective)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr StreamCompress(CCtx* ctx, byte* dst, UIntPtr dstCapacity, UIntPtr* dstPos, byte* src, UIntPtr srcSize, UIntPtr* srcPos, EndDirective endOp) {
        IL.Push(ctx);
        IL.Push(dst);
        IL.Push(dstCapacity);
        IL.Push(dstPos);
        IL.Push(src);
        IL.Push(srcSize);
        IL.Push(srcPos);
        IL.Push(endOp);
        IL.Push(ZSTD_compressStream2_simpleArgs);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*),
          typeof(byte*), typeof(UIntPtr), typeof(UIntPtr*),
          typeof(byte*), typeof(UIntPtr), typeof(UIntPtr*),
          typeof(EndDirective)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr StreamCompress(CCtx* ctx, ref Buffer output, ref Buffer input, EndDirective endOp) {
        fixed (Buffer* pOutput = &output)
        fixed (Buffer* pInput = &input) {
          return StreamCompress(
            ctx,
            pOutput,
            pInput,
            endOp
          );
        }
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr StreamCompress(CCtx* ctx, ref ArraySegment<byte> output, ref ArraySegment<byte> input, EndDirective endOp) {
        UIntPtr result = default;

        Buffer.WithArraySegmentPair(ref output, ref input, (ref Buffer outBuf, ref Buffer inBuf) => {
          result = StreamCompress(ctx, ref outBuf, ref inBuf, endOp);
        });

        return result;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr Copy(CCtx** cCtx, CCtx* preparedCCtx, ulong pledgedSrcSize = ulong.MaxValue) {
        IL.Push(cCtx);
        IL.Push(preparedCCtx);
        IL.Push(pledgedSrcSize);
        IL.Push(ZSTD_copyCCtx);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx**), typeof(CCtx*), typeof(ulong)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static CCtx* Copy(CCtx* ctx, ulong pledgedSrcSize = ulong.MaxValue) {
        CCtx* newCtx;
        Copy(&newCtx, ctx, pledgedSrcSize).EnsureZStdSuccess();
        return newCtx;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr GetBytesReadyToFlush(CCtx* cCtx) {
        IL.Push(cCtx);
        IL.Push(ZSTD_toFlushNow);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*)));
        return IL.Return<UIntPtr>();
      }

    }

  }

}
