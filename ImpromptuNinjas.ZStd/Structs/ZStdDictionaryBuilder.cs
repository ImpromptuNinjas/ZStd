using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.IO;
using static ImpromptuNinjas.ZStd.Native;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public partial struct ZStdDictionaryBuilder : ICloneable {

    public static RecyclableMemoryStreamManager RecyclableMemoryStreamManager = new RecyclableMemoryStreamManager
      (512, 8192, 1048576, false) {
        AggressiveBufferReturn = true,
        GenerateCallStacks = false,
        MaximumFreeSmallPoolBytes = 4194304,
        MaximumFreeLargePoolBytes = 4194304
      };

    public readonly byte[] Data;

    public UIntPtr Size;

    private ZStdDictionaryBuilder(byte[] data, in UIntPtr size)
      => (Data, Size) = (data, size);

    public static implicit operator Span<byte>(ZStdDictionaryBuilder dict)
      => new Span<byte>(dict.Data);

    public static implicit operator ReadOnlySpan<byte>(ZStdDictionaryBuilder dict)
      => new ReadOnlySpan<byte>(dict.Data, 0, (int) dict.Size);

    public static implicit operator ArraySegment<byte>(ZStdDictionaryBuilder db)
      => new ArraySegment<byte>(db.Data, 0, (int) db.Size);

    public ZStdDictionaryBuilder(byte[] buffer)
      => (Data, Size) = (buffer, (UIntPtr) buffer.Length);

    public unsafe ZStdDictionaryBuilder(UIntPtr size)
      => (Data, Size) = (sizeof(UIntPtr) == 8 ? new byte[size.ToUInt64()] : new byte[size.ToUInt32()], default);

    public ZStdDictionaryBuilder(long size)
      => (Data, Size) = (new byte[size], default);

    public ZStdDictionaryBuilder(ulong size)
      => (Data, Size) = (new byte[size], default);

    public int WriteTo(Stream s) {
      s.Write(Data, 0, (int) Size);
      return 0;
    }

    [MustUseReturnValue]
    private static (ArraySegment<byte> Samples, UIntPtr[] SamplesSizes) GatherSamples(SamplerDelegate sampler) {
      using var stream = RecyclableMemoryStreamManager.GetStream("ZStd Dictionary Sampling Buffer");
      var sizes = new LinkedList<UIntPtr>();

      foreach (var sample in sampler()) {
        stream.Write(sample);
        sizes.AddLast((UIntPtr) (sample.Count - sample.Offset));
      }

      var sizesArray = new UIntPtr[sizes.Count];
      sizes.CopyTo(sizesArray, 0);

      return (new ArraySegment<byte>(stream.GetBuffer(), 0, (int) stream.Length), sizesArray);
    }

    [MustUseReturnValue]
    private static async Task<(ArraySegment<byte> Samples, UIntPtr[] SamplesSizes)> GatherSamples(AsyncSamplerDelegate sampler) {
      await using var stream = RecyclableMemoryStreamManager.GetStream("ZStd Dictionary Sampling Buffer");
      var sizes = new LinkedList<UIntPtr>();

      await foreach (var sample in sampler()) {
        stream.Write(sample);
        sizes.AddLast((UIntPtr) (sample.Count - sample.Offset));
      }

      var sizesArray = new UIntPtr[sizes.Count];
      sizes.CopyTo(sizesArray, 0);

      return (new ArraySegment<byte>(stream.GetBuffer(), 0, (int) stream.Length), sizesArray);
    }

    private static DictionaryTrainingParameters GetDefaultTrainingParameters(int compressionLevel, uint nbThreads, uint tuningSteps) {
      var parameters = new DictionaryTrainingParameters {
        StandardParameters = {
          CompressionLevel = compressionLevel,
          NotificationLevel = 2
        },
        DmerSize = 8,
        SamplePortion = 1,
        ThreadCount = nbThreads,
        Steps = tuningSteps
      };
      return parameters;
    }

    [MustUseReturnValue]
    public uint GetId()
      => ZDict.GetId(Data);

    /*
    [MustUseReturnValue]
    public UIntPtr GetHeaderSize()
      => ZDict.GetHeaderSize(Data);
    */

    [MustUseReturnValue]
    public bool IsValid()
      => GetId() != 0u;

    public unsafe ZStdDictionaryBuilder Clone() {
      var copy = sizeof(UIntPtr) == 8 ? new byte[Size.ToUInt64()] : new byte[Size.ToUInt32()];
      Unsafe.CopyBlock(ref copy[0], ref Data[0], (uint) Size);
      return new ZStdDictionaryBuilder(copy, Size);
    }

    object ICloneable.Clone()
      => Clone();

    public ZStdCompressorDictionary CreateCompressorDictionary(int compressionLevel = default)
      => new ZStdCompressorDictionary(this, compressionLevel);

    public ZStdDecompressorDictionary CreateDecompressorDictionary()
      => new ZStdDecompressorDictionary(this);

  }

}
