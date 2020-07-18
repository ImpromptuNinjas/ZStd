using System;
using System.Linq;
using FluentAssertions;
#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
#endif


namespace ImpromptuNinjas.ZStd.Tests {

  public partial class BindingTests {

#if MSTEST
    [TestMethod,UseParameterValues]
#else
    [Test]
#endif
    public void FrameRoundTrip(
      [ValueSource(typeof(Utilities), nameof(Utilities.CompressionLevels))]
      int compressionLevel,
      [Values(false, true)] bool useDictionary
    ) {
#pragma warning disable 8632
      var dict = useDictionary
        ? Utilities.CreateDictionaryFromSamples(32 * 1024, 30, compressionLevel)
        // ReSharper disable once RedundantCast // downlevel impl requires
        : (ZStdDictionaryBuilder?) null;
#pragma warning restore 8632

      var sample = Utilities.GenerateSampleBuffer(1000);

      Console.WriteLine($"Compression Level: {compressionLevel}");

      // compression

      using var cDict = dict?.CreateCompressorDictionary(compressionLevel);

      using var cCtx = new ZStdCompressor();

      cCtx.UseDictionary(cDict);

      var compressBufferSize = CCtx.GetUpperBound((UIntPtr) sample.Length).ToUInt32();

      var compressBuffer = new byte[compressBufferSize];

      cCtx.Set(CompressionParameter.CompressionLevel, compressionLevel);

      var compressedSize = cCtx.Compress(compressBuffer, sample);

      compressedSize.ToUInt64().Should().NotBe(0);

      var compressedFrame = new ArraySegment<byte>(compressBuffer, 0, (int) compressedSize);

      Console.WriteLine($"Compressed to: {compressedSize} ({(double) compressedSize / (double) sample.Length:P})");

      // decompression

      using var dDict = dict?.CreateDecompressorDictionary();

      using var dCtx = new ZStdDecompressor();

      dCtx.UseDictionary(dDict);

      var decompressBufferSize = DCtx.GetUpperBound(compressedFrame);

      decompressBufferSize.Should().NotBe(0);

      decompressBufferSize.Should().BeGreaterOrEqualTo((ulong)sample.Length);

      var decompressBuffer = new byte[decompressBufferSize];

      var decompressedSize = dCtx.Decompress(decompressBuffer, compressedFrame);

      (decompressedSize).Should().Be((UIntPtr) sample.Length);

      decompressBuffer.Take((int) decompressedSize).Should().Equal(sample);
    }

  }

}
