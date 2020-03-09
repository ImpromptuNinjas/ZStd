using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace ImpromptuNinjas.ZStd {

  public static unsafe partial class Native {

    public static class ZStdCDict {

      static ZStdCDict()
        => Init();

      #region Dynamic Library Import Table

      // ReSharper disable IdentifierTypo
      // ReSharper disable StringLiteralTypo
      // ReSharper disable InconsistentNaming

      private static readonly IntPtr ZSTD_createCDict = NativeLibrary.GetExport(Lib, nameof(ZSTD_createCDict));

      private static readonly IntPtr ZSTD_freeCDict = NativeLibrary.GetExport(Lib, nameof(ZSTD_freeCDict));

      private static readonly IntPtr ZSTD_sizeof_CDict = NativeLibrary.GetExport(Lib, nameof(ZSTD_sizeof_CDict));

      // ReSharper restore InconsistentNaming
      // ReSharper restore StringLiteralTypo
      // ReSharper restore IdentifierTypo

      #endregion

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static CDict* CreateCDict(byte* dict, UIntPtr dictSize, int compressionLevel) {
        IL.Push(dict);
        IL.Push(dictSize);
        IL.Push(compressionLevel);
        IL.Push(ZSTD_createCDict);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(CDict*),
          typeof(byte*), typeof(UIntPtr), typeof(int)));
        return IL.ReturnPointer<CDict>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static CDict* CreateCDict(ReadOnlySpan<byte> dict, int compressionLevel) {
        fixed (byte* pDict = dict)
          return CreateCDict(pDict, (UIntPtr) dict.Length, compressionLevel);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr FreeCDict(CDict* cDict) {
        IL.Push(cDict);
        IL.Push(ZSTD_freeCDict);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CDict*)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr SizeOfCDict(CDict* cDict) {
        IL.Push(cDict);
        IL.Push(ZSTD_sizeof_CDict);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(CDict*)));
        return IL.Return<UIntPtr>();
      }

    }

  }

}
