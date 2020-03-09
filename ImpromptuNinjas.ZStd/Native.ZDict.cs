using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;
using JetBrains.Annotations;
using static InlineIL.IL;
using static InlineIL.IL.Emit;
using size_t = System.UIntPtr;

namespace ImpromptuNinjas.ZStd {

  public static unsafe partial class Native {

    [PublicAPI]
    public static class ZDict {

      #region Dynamic Library Import Table

      // ReSharper disable IdentifierTypo
      // ReSharper disable StringLiteralTypo
      // ReSharper disable InconsistentNaming

      //private static readonly IntPtr ZDICT_getDictHeaderSize = NativeLibrary.GetExport(LoadedLib, nameof(ZDICT_getDictHeaderSize));

      private static readonly IntPtr ZDICT_getDictID = NativeLibrary.GetExport(Lib, nameof(ZDICT_getDictID));

      private static readonly IntPtr ZDICT_getErrorName = NativeLibrary.GetExport(Lib, nameof(ZDICT_getErrorName));

      private static readonly IntPtr ZDICT_isError = NativeLibrary.GetExport(Lib, nameof(ZDICT_isError));

      private static readonly IntPtr ZDICT_optimizeTrainFromBuffer_fastCover = NativeLibrary.GetExport(Lib, nameof(ZDICT_optimizeTrainFromBuffer_fastCover));

      // ReSharper restore InconsistentNaming
      // ReSharper restore StringLiteralTypo
      // ReSharper restore IdentifierTypo

      #endregion

      static ZDict() => Init();

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static size_t Train(byte* dictBuffer, size_t dictBufferCapacity, byte* samplesBuffer, size_t* samplesSizes, uint nbSamples, DictionaryTrainingParameters* parameters) {
        Push(dictBuffer);
        Push(dictBufferCapacity);
        Push(samplesBuffer);
        Push(samplesSizes);
        Push(nbSamples);
        Push(parameters);
        Push(ZDICT_optimizeTrainFromBuffer_fastCover);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(size_t),
          typeof(byte*), typeof(size_t), typeof(byte*), typeof(size_t), typeof(uint), typeof(DictionaryTrainingParameters*)));
        return Return<size_t>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static size_t Train(Span<byte> dictionary, ReadOnlySpan<byte> samples, ReadOnlySpan<size_t> samplesSizes, ref DictionaryTrainingParameters parameters) {
        fixed (byte* pDictBuffer = dictionary)
        fixed (byte* pSamplesBuffer = samples)
        fixed (size_t* pSamplesSizes = samplesSizes)
        fixed (DictionaryTrainingParameters* pParameters = &parameters)
          return Train(pDictBuffer, (size_t) dictionary.Length, pSamplesBuffer, pSamplesSizes, (uint) samplesSizes.Length, pParameters);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static uint GetId(byte* dictBuffer, size_t dictSize) {
        Push(dictBuffer);
        Push(dictSize);
        Push(ZDICT_getDictID);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(uint),
          typeof(byte*), typeof(size_t)));
        return Return<uint>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static uint GetId(ReadOnlySpan<byte> dictBuffer) {
        fixed (byte* pDictBuffer = dictBuffer)
          return GetId(pDictBuffer, (UIntPtr) dictBuffer.Length);
      }

      /*
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static size_t GetHeaderSize(byte* dictBuffer, size_t dictSize) {
        Push(dictBuffer);
        Push(dictSize);
        Push(ZDICT_getDictHeaderSize);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(size_t),
          typeof(byte*), typeof(size_t)));
        return Return<size_t>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static size_t GetHeaderSize(ReadOnlySpan<byte> dictBuffer) {
        fixed (byte* pDictBuffer = dictBuffer)
          return GetHeaderSize(pDictBuffer, (UIntPtr) dictBuffer.Length);
      }
      */

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static uint IsError(size_t code) {
        Push(code);
        Push(ZDICT_isError);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(uint),
          typeof(size_t)));
        return Return<uint>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      internal static sbyte* GetErrorNameInternal(size_t code) {
        Push(code);
        Push(ZDICT_getErrorName);
        Tail();
        Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(sbyte*),
          typeof(size_t)));
        return ReturnPointer<sbyte>();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static string GetErrorName(size_t code)
        => new string(GetErrorNameInternal(code));

    }

  }

}
