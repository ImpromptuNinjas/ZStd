using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public class ZstdDictionaryTrainer : IEnumerable<ArraySegment<byte>> {

    public ZStdDictionaryBuilder DictionaryBuilder {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get;
    }

    private DictionaryTrainingParameters _parameters;

    private int _offset;

    private byte[] _buffer;

    private int _sampleCount;

    private UIntPtr[] _sizes;

    public ref DictionaryTrainingParameters TrainingParameters {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => ref _parameters;
    }

    public ref DictionaryParameters StandardParameters {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => ref _parameters.StandardParameters;
    }

    public ReadOnlySpan<UIntPtr> SamplesSizes {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new ReadOnlySpan<UIntPtr>(_sizes, 0, _sampleCount);
    }

    public ReadOnlySpan<byte> SampleBuffer {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new ReadOnlySpan<byte>(_buffer, 0, _offset);
    }

    public ZstdDictionaryTrainer(ZStdDictionaryBuilder dictionaryBuilder, in DictionaryTrainingParameters parameterTemplate) {
      _parameters = parameterTemplate;
      DictionaryBuilder = dictionaryBuilder;
    }

    public void EnsureBufferCapacity(int requiredSize) {
      var paddedSize = (requiredSize + 4095) / 4096 * 4096;
      if (_buffer == null)
        _buffer = new byte[paddedSize];
      else if (_buffer.Length < requiredSize) {
        var oldBuf = _buffer;
        _buffer = new byte[paddedSize];
        Unsafe.CopyBlock(ref _buffer[0], ref oldBuf[0], (uint) _offset);
      }
    }

    public unsafe void EnsureSampleCapacity(int sampleCount) {
      var paddedCount = (sampleCount + 128) / 128 * 128;
      if (_sizes == null)
        _sizes = new UIntPtr[paddedCount];
      else if (_sizes.Length < sampleCount) {
        var oldSizes = _sizes;
        _sizes = new UIntPtr[paddedCount];

        fixed (UIntPtr* pOldSizes = oldSizes)
        fixed (UIntPtr* pNewSizes = _sizes)
          Unsafe.CopyBlock(pNewSizes, pOldSizes, (uint) (_sampleCount * sizeof(UIntPtr)));
      }
    }

    public void Sample(byte[] sample, int offset, int length)
      => Sample(new ArraySegment<byte>(sample, offset, length));

#if NETSTANDARD2_1 || NETCOREAPP
    public void Sample(ref byte sample, int length)
      => Sample(MemoryMarshal.CreateReadOnlySpan(ref sample, length));
#endif

    public void Sample(ArraySegment<byte> sample) {
      if (sample.Array == null)
        throw new ArgumentNullException(nameof(sample), "Array segment is missing Array.");

      var offset = _offset;
      var sampleSize = sample.Count;
      EnsureBufferCapacity(offset + sampleSize);
      EnsureSampleCapacity(_sampleCount + 1);

      Unsafe.CopyBlockUnaligned(ref _buffer[offset], ref sample.Array![sample.Offset], (uint) sampleSize);
      _offset += sampleSize;
      _sizes[_sampleCount++] = (UIntPtr) sampleSize;
    }

    public unsafe void Sample(ReadOnlySpan<byte> sample) {
      var offset = _offset;
      var sampleSize = sample.Length;
      EnsureBufferCapacity(offset + sampleSize);
      EnsureSampleCapacity(_sampleCount + 1);

      fixed (byte* pBuffer = _buffer)
      fixed (byte* pSample = sample)
        Unsafe.CopyBlockUnaligned(&pBuffer[offset], pSample, (uint) sampleSize);
      _offset += sampleSize;
      _sizes[_sampleCount++] = (UIntPtr) sampleSize;
    }

    public unsafe void Sample(byte * pSample, int sampleSize) {
      var offset = _offset;
      EnsureBufferCapacity(offset + sampleSize);
      EnsureSampleCapacity(_sampleCount + 1);

      fixed (byte* pBuffer = _buffer)
        Unsafe.CopyBlockUnaligned(&pBuffer[offset], pSample, (uint) sampleSize);
      _offset += sampleSize;
      _sizes[_sampleCount++] = (UIntPtr) sampleSize;
    }

    public void Train(bool dontReset = false) {
      DictionaryBuilder.Size = Native.ZDict.Train(
          DictionaryBuilder.Data,
          SampleBuffer,
          SamplesSizes,
          ref _parameters
        )
        .EnsureZDictSuccess();

      if (!dontReset)
        Reset();
    }

    public void Reset() {
      _offset = 0;
      _sampleCount = 0;
    }

    public IEnumerator<ArraySegment<byte>> GetEnumerator() {
      var sampleCount = _sampleCount;
      var sampled = _offset;
      var buffer = _buffer;
      var sizes = _sizes;

      var offset = 0;
      for (var i = 0; i < sampleCount; ++i) {
        if (sampled != _offset || sampleCount != _sampleCount)
          throw new InvalidOperationException("Collection was modified.");

        var sampleSize = (int) sizes[i];
        yield return new ArraySegment<byte>(buffer, offset, sampleSize);

        offset += sampleSize;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
      => GetEnumerator();

  }

}
