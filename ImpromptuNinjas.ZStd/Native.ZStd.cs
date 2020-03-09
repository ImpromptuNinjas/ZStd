using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;
using JetBrains.Annotations;
using static InlineIL.IL;
using static InlineIL.IL.Emit;

namespace ImpromptuNinjas.ZStd {

  public static unsafe partial class Native {

    [PublicAPI]
    public static class ZStd {

      static ZStd()
        => Init();

      #region Dynamic Library Import Table

      // ReSharper disable IdentifierTypo
      // ReSharper disable StringLiteralTypo
      // ReSharper disable InconsistentNaming

      private static readonly IntPtr ZSTD_decompressBound = NativeLibrary.GetExport(Lib, nameof(ZSTD_decompressBound));

      private static readonly IntPtr ZSTD_findDecompressedSize = NativeLibrary.GetExport(Lib, nameof(ZSTD_findDecompressedSize));

      private static readonly IntPtr ZSTD_findFrameCompressedSize = NativeLibrary.GetExport(Lib, nameof(ZSTD_findFrameCompressedSize));

      private static readonly IntPtr ZSTD_getDecompressedSize = NativeLibrary.GetExport(Lib, nameof(ZSTD_getDecompressedSize));

      private static readonly IntPtr ZSTD_getErrorName = NativeLibrary.GetExport(Lib, nameof(ZSTD_getErrorName));

      private static readonly IntPtr ZSTD_getFrameContentSize = NativeLibrary.GetExport(Lib, nameof(ZSTD_getFrameContentSize));

      private static readonly IntPtr ZSTD_isError = NativeLibrary.GetExport(Lib, nameof(ZSTD_isError));

      private static readonly IntPtr ZSTD_maxCLevel = NativeLibrary.GetExport(Lib, nameof(ZSTD_maxCLevel));

      private static readonly IntPtr ZSTD_minCLevel = NativeLibrary.GetExport(Lib, nameof(ZSTD_minCLevel));

      private static readonly IntPtr ZSTD_versionNumber = NativeLibrary.GetExport(Lib, nameof(ZSTD_versionNumber));

      private static readonly IntPtr ZSTD_versionString = NativeLibrary.GetExport(Lib, nameof(ZSTD_versionString));

      // ReSharper restore InconsistentNaming
      // ReSharper restore StringLiteralTypo
      // ReSharper restore IdentifierTypo

      #endregion

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static ulong FindDecompressedSize(byte* src, UIntPtr srcSize) {
        Push(src);
        Push(srcSize);
        Push(ZSTD_findDecompressedSize);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(ulong),
          typeof(byte*), typeof(UIntPtr)));
        return Return<ulong>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ulong FindDecompressedSize(ReadOnlySpan<byte> src) {
        fixed (byte* pSrc = src)
          return FindDecompressedSize(pSrc, (UIntPtr) src.Length);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static ulong GetDecompressedSize(byte* src, UIntPtr srcSize) {
        Push(src);
        Push(srcSize);
        Push(ZSTD_getDecompressedSize);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(ulong),
          typeof(byte*), typeof(UIntPtr)));
        return Return<ulong>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ulong GetDecompressedSize(ReadOnlySpan<byte> src) {
        fixed (byte* pSrc = src)
          return GetDecompressedSize(pSrc, (UIntPtr) src.Length);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static ulong DecompressBound(byte* src, UIntPtr srcSize) {
        Push(src);
        Push(srcSize);
        Push(ZSTD_decompressBound);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(ulong),
          typeof(byte*), typeof(UIntPtr)));
        return Return<ulong>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ulong DecompressBound(ReadOnlySpan<byte> src) {
        fixed (byte* pSrc = src)
          return DecompressBound(pSrc, (UIntPtr) src.Length);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static uint IsError(UIntPtr code) {
        Push(code);
        Push(ZSTD_isError);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(uint),
          typeof(UIntPtr)));
        return Return<uint>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      internal static sbyte* GetErrorNameInternal(UIntPtr code) {
        Push(code);
        Push(ZSTD_getErrorName);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(sbyte*),
          typeof(UIntPtr)));
        return ReturnPointer<sbyte>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static string GetErrorName(UIntPtr code)
        => new string(GetErrorNameInternal(code));

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr GetFrameContentSize(byte* src, UIntPtr srcSize) {
        Push(src);
        Push(srcSize);
        Push(ZSTD_getFrameContentSize);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(byte*), typeof(UIntPtr)));
        return Return<UIntPtr>();
      }

      public static UIntPtr GetFrameContentSize(ReadOnlySpan<byte> src) {
        fixed (byte* pSrc = src)
          return GetFrameContentSize(pSrc, (UIntPtr) src.Length);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static UIntPtr FindFrameCompressedSize(byte* src, UIntPtr srcSize) {
        Push(src);
        Push(srcSize);
        Push(ZSTD_findFrameCompressedSize);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(byte*), typeof(UIntPtr)));
        return Return<UIntPtr>();
      }

      public static UIntPtr FindFrameCompressedSize(ReadOnlySpan<byte> src) {
        fixed (byte* pSrc = src)
          return FindFrameCompressedSize(pSrc, (UIntPtr) src.Length);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static int GetMaxCompressionLevel() {
        Push(ZSTD_maxCLevel);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
        return Return<int>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static int GetMinCompressionLevel() {
        Push(ZSTD_minCLevel);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
        return Return<int>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static uint GetVersionNumber() {
        Push(ZSTD_versionNumber);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(uint)));
        return Return<uint>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static sbyte* GetVersionStringInternal() {
        Push(ZSTD_versionString);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(sbyte*)));
        return ReturnPointer<sbyte>();
      }

      public static string GetVersionString()
        => new string(GetVersionStringInternal());

    }

  }

}
