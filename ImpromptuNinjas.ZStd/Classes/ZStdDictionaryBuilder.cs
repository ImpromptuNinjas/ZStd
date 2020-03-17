using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
#if !NETSTANDARD1_1
using Microsoft.IO;
#endif
using static ImpromptuNinjas.ZStd.Native;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public partial class ZStdDictionaryBuilder
#if !NETSTANDARD1_4 && !NETSTANDARD1_1
    : ICloneable
#endif
  {

#if !NETSTANDARD1_1
    public static RecyclableMemoryStreamManager RecyclableMemoryStreamManager = new RecyclableMemoryStreamManager
      (512, 8192, 1048576, false) {
        AggressiveBufferReturn = true,
        GenerateCallStacks = false,
        MaximumFreeSmallPoolBytes = 4194304,
        MaximumFreeLargePoolBytes = 4194304
      };
#endif

    public readonly byte[] Data;

    public UIntPtr Size;

    private ZStdDictionaryBuilder(byte[] data, in UIntPtr size) {
      Data = data;
      Size = size;
    }

    public static implicit operator Span<byte>(ZStdDictionaryBuilder dict)
      => new Span<byte>(dict.Data);

    public static implicit operator ReadOnlySpan<byte>(ZStdDictionaryBuilder dict)
      => new ReadOnlySpan<byte>(dict.Data, 0, (int) dict.Size);

    public static implicit operator ArraySegment<byte>(ZStdDictionaryBuilder db)
      => new ArraySegment<byte>(db.Data, 0, (int) db.Size);

    public ZStdDictionaryBuilder(byte[] buffer) {
      Data = buffer;
      Size = (UIntPtr) buffer.Length;
    }

    public unsafe ZStdDictionaryBuilder(UIntPtr size) {
      Data = sizeof(UIntPtr) == 8 ? new byte[size.ToUInt64()] : new byte[size.ToUInt32()];
      Size = default;
    }

    public ZStdDictionaryBuilder(long size) {
      Data = new byte[size];
      Size = default;
    }

    public ZStdDictionaryBuilder(ulong size) {
      Data = new byte[size];
      Size = default;
    }

    public int WriteTo(Stream s) {
      s.Write(Data, 0, (int) Size);
      return 0;
    }

    [MustUseReturnValue]
    private static ArraySegment<byte> GatherSamples(SamplerDelegate sampler, out UIntPtr[] sampleSizes) {
#if !NETSTANDARD1_1
      using var stream = (RecyclableMemoryStream) RecyclableMemoryStreamManager
        .GetStream("ZStd Dictionary Sampling Buffer");
#else
      using var stream = new MemoryStream();
#endif

      var sizes = new LinkedList<UIntPtr>();

      foreach (var sample in sampler()) {
        if (sample.Array == null)
          continue;

        stream.Write(sample.Array, sample.Offset, sample.Count);
        sizes.AddLast((UIntPtr) (sample.Count - sample.Offset));
      }

      sampleSizes = new UIntPtr[sizes.Count];
      sizes.CopyTo(sampleSizes, 0);

#if !NETSTANDARD1_1
      return new ArraySegment<byte>(stream.GetBuffer(), 0, (int) stream.Length);
#else
      return new ArraySegment<byte>(stream.ToArray(), 0, (int) stream.Length);
#endif
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

#if !NETSTANDARD1_4 && !NETSTANDARD1_1
    object ICloneable.Clone()
      => Clone();
#endif

    public ZStdCompressorDictionary CreateCompressorDictionary(int compressionLevel = default)
      => new ZStdCompressorDictionary(this, compressionLevel);

    public ZStdDecompressorDictionary CreateDecompressorDictionary()
      => new ZStdDecompressorDictionary(this);

  }

}
