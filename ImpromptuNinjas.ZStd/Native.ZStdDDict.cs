using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace ImpromptuNinjas.ZStd {

  public static unsafe partial class Native {

    public static class ZStdDDict {

      static ZStdDDict()
        => Init();

      #region Dynamic Library Import Table

      // ReSharper disable IdentifierTypo
      // ReSharper disable StringLiteralTypo
      // ReSharper disable InconsistentNaming

      private static readonly IntPtr ZSTD_createDDict = NativeLibrary.GetExport(Lib, nameof(ZSTD_createDDict));

      private static readonly IntPtr ZSTD_freeDDict = NativeLibrary.GetExport(Lib, nameof(ZSTD_freeDDict));

      private static readonly IntPtr ZSTD_sizeof_DDict = NativeLibrary.GetExport(Lib, nameof(ZSTD_sizeof_DDict));

      // ReSharper restore InconsistentNaming
      // ReSharper restore StringLiteralTypo
      // ReSharper restore IdentifierTypo

      #endregion

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static DDict* CreateDDict(byte* dict, UIntPtr dictSize) {
        IL.Push(dict);
        IL.Push(dictSize);
        IL.Push(ZSTD_createDDict);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(DDict*),
          typeof(byte*), typeof(UIntPtr)));
        return IL.ReturnPointer<DDict>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static DDict* CreateDDict(ReadOnlySpan<byte> dict) {
        fixed (byte* pDict = dict)
          return CreateDDict(pDict, (UIntPtr) dict.Length);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr FreeDDict(DDict* dDict) {
        IL.Push(dDict);
        IL.Push(ZSTD_freeDDict);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(DDict*)));
        return IL.Return<UIntPtr>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static UIntPtr SizeOfDDict(DDict* dDict) {
        IL.Push(dDict);
        IL.Push(ZSTD_sizeof_DDict);
        IL.Emit.Tail();
        IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(UIntPtr),
          typeof(DDict*)));
        return IL.Return<UIntPtr>();
      }

    }

  }

}
