using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  public static unsafe partial class Native {

    [PublicAPI]
    public static class ZStdBlock {

      static ZStdBlock()
        => Init();

      #region Dynamic Library Import Table

      // ReSharper disable IdentifierTypo
      // ReSharper disable StringLiteralTypo
      // ReSharper disable InconsistentNaming

      private static readonly IntPtr ZSTD_compressBlock = NativeLibrary.GetExport(Lib, nameof(ZSTD_compressBlock));

      private static readonly IntPtr ZSTD_decompressBlock = NativeLibrary.GetExport(Lib, nameof(ZSTD_decompressBlock));

      private static readonly IntPtr ZSTD_getBlockSize = NativeLibrary.GetExport(Lib, nameof(ZSTD_getBlockSize));

      private static readonly IntPtr ZSTD_insertBlock = NativeLibrary.GetExport(Lib, nameof(ZSTD_insertBlock));

      // ReSharper restore InconsistentNaming
      // ReSharper restore StringLiteralTypo
      // ReSharper restore IdentifierTypo

      #endregion

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr GetBlockSize(CCtx* ctx) {
        IL.Push(ctx);
        IL.Push(ZSTD_getBlockSize);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr CompressBlock(CCtx* ctx, byte* dst, UIntPtr dstSize, byte* src, UIntPtr srcSize) {
        IL.Push(ctx);
        IL.Push(dst);
        IL.Push(dstSize);
        IL.Push(src);
        IL.Push(srcSize);
        IL.Push(ZSTD_compressBlock);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(byte*), typeof(UIntPtr), typeof(byte*), typeof(UIntPtr)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr DecompressBlock(DCtx* ctx, byte* dst, UIntPtr dstSize, byte* src, UIntPtr srcSize) {
        IL.Push(ctx);
        IL.Push(dst);
        IL.Push(dstSize);
        IL.Push(src);
        IL.Push(srcSize);
        IL.Push(ZSTD_decompressBlock);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(DCtx*), typeof(byte*), typeof(UIntPtr), typeof(byte*), typeof(UIntPtr)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr InsertBlock(DCtx* ctx, byte* src, UIntPtr srcSize) {
        IL.Push(ctx);
        IL.Push(src);
        IL.Push(srcSize);
        IL.Push(ZSTD_insertBlock);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CCtx*), typeof(byte*), typeof(UIntPtr)));
        return IL.Return<UIntPtr>();
      }

    }

  }

}
