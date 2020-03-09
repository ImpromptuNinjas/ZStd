using System;
using System.Linq;
using NUnit.Framework;

namespace ImpromptuNinjas.ZStd.Tests {

  public partial class BindingTests {

    [Test]
    public void StreamRoundTrip(
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

      UIntPtr compressedSize;

      cCtx.Set(CompressionParameter.CompressionLevel, compressionLevel);
      {
        var outBuf = new ArraySegment<byte>(compressBuffer, 0, compressBuffer.Length);
        var inBuf = new ArraySegment<byte>(sample, 0, sample.Length);

        var amountRemaining = cCtx.StreamCompress(ref outBuf, ref inBuf, EndDirective.End);

        Assert.Zero(amountRemaining.ToUInt64());

        Assert.NotZero(outBuf.Offset);
        Assert.NotZero(inBuf.Offset);
        Assert.AreEqual(sample.Length, inBuf.Offset);

        compressedSize = (UIntPtr) outBuf.Offset;
      }

      var compressedFrame = new ArraySegment<byte>(compressBuffer, 0, (int) compressedSize);

      Console.WriteLine($"Compressed to: {compressedSize} ({(double) compressedSize / (double) sample.Length:P})");

      // decompression

      using var dDict = dict?.CreateDecompressorDictionary();

      using var dCtx = new ZStdDecompressor();

      dCtx.UseDictionary(dDict);

      var decompressBufferSize = DCtx.GetUpperBound(compressedFrame);

      var decompressBuffer = new byte[decompressBufferSize];
      {
        var outBuf = new ArraySegment<byte>(decompressBuffer, 0, decompressBuffer.Length);
        var inBuf = compressedFrame;

        var amountRemaining = dCtx.StreamDecompress(ref outBuf, ref inBuf);

        Assert.Zero(amountRemaining.ToUInt64());
        Assert.Zero(outBuf.Count);
        Assert.Zero(inBuf.Count);
        Assert.NotZero(outBuf.Offset);
        Assert.NotZero(inBuf.Offset);
        Assert.AreEqual(sample.Length, outBuf.Offset);

        CollectionAssert.AreEqual(sample, decompressBuffer.Take(outBuf.Offset));
      }
    }

  }

}
