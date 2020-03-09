using System;
using System.Linq;
using NUnit.Framework;

namespace ImpromptuNinjas.ZStd.Tests {

  public partial class BindingTests {

    [Test]
    public void FrameRoundTrip(
      [ValueSource(typeof(Utilities), nameof(Utilities.CompressionLevels))]
      int compressionLevel
    ) {
      var sample = Utilities.GenerateSampleBuffer(1000);

      Console.WriteLine($"Compression Level: {compressionLevel}");

      // compression

      using var cCtx = new ZStdCompressor();

      var compressBufferSize = CCtx.GetUpperBound((UIntPtr) sample.Length);

      var compressBuffer = new byte[compressBufferSize];

      cCtx.Set(CompressionParameter.CompressionLevel, compressionLevel);
      var compressedSize = cCtx.Compress(compressBuffer, sample);

      Assert.NotZero(compressedSize.ToUInt64());

      var compressedFrame = new ArraySegment<byte>(compressBuffer, 0, (int) compressedSize);

      Console.WriteLine($"Compressed to: {compressedSize} ({(double) compressedSize / (double) sample.Length:P})");

      // decompression

      using var dCtx = new ZStdDecompressor();

      var decompressBufferSize = DCtx.GetUpperBound(compressedFrame);

      Assert.NotZero(decompressBufferSize);

      Assert.GreaterOrEqual(decompressBufferSize, (ulong) sample.Length);

      var decompressBuffer = new byte[decompressBufferSize];
      var decompressedSize = dCtx.Decompress(decompressBuffer, compressedFrame);

      Assert.AreEqual((UIntPtr) sample.Length, decompressedSize);
      CollectionAssert.AreEqual(sample, decompressBuffer.Take((int) decompressedSize));
    }

    [Test]
    public void FrameRoundTripWithDictionary(
      [ValueSource(typeof(Utilities), nameof(Utilities.CompressionLevels))]
      int compressionLevel,
      [Values(false, true)] bool useDictionary
    ) {
      var dict = useDictionary
        ? Utilities.CreateDictionaryFromSamples(32 * 1024, 30, compressionLevel)
        : (ZStdDictionaryBuilder?) null;

      var sample = Utilities.GenerateSampleBuffer(1000);

      Console.WriteLine($"Compression Level: {compressionLevel}");

      // compression

      using var cDict = dict?.CreateCompressorDictionary(compressionLevel);

      using var cCtx = new ZStdCompressor();

      cCtx.UseDictionary(cDict);

      var compressBufferSize = CCtx.GetUpperBound((UIntPtr) sample.Length);

      var compressBuffer = new byte[compressBufferSize];

      cCtx.Set(CompressionParameter.CompressionLevel, compressionLevel);

      var compressedSize = cCtx.Compress(compressBuffer, sample);

      Assert.NotZero(compressedSize.ToUInt64());

      var compressedFrame = new ArraySegment<byte>(compressBuffer, 0, (int) compressedSize);

      Console.WriteLine($"Compressed to: {compressedSize} ({(double) compressedSize / (double) sample.Length:P})");

      // decompression

      var dDict = dict?.CreateDecompressorDictionary();

      var dCtx = new ZStdDecompressor();

      dCtx.UseDictionary(dDict);

      var decompressBufferSize = DCtx.GetUpperBound(compressedFrame);

      Assert.NotZero(decompressBufferSize);

      Assert.GreaterOrEqual(decompressBufferSize, (ulong) sample.Length);

      var decompressBuffer = new byte[decompressBufferSize];

      var decompressedSize = dCtx.Decompress(decompressBuffer, compressedFrame);

      Assert.AreEqual((UIntPtr) sample.Length, decompressedSize);
      CollectionAssert.AreEqual(sample, decompressBuffer.Take((int) decompressedSize));
    }

  }

}
