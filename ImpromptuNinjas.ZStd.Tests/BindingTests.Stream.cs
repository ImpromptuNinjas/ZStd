using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        amountRemaining.ToUInt64().Should().Be(0);

        outBuf.Offset.Should().NotBe(0);
        inBuf.Offset.Should().NotBe(0);
        inBuf.Offset.Should().Be(sample.Length);

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

        amountRemaining.ToUInt64().Should().Be(0);
        outBuf.Count.Should().Be(0);
        inBuf.Count.Should().Be(0);
        outBuf.Offset.Should().NotBe(0);
        inBuf.Offset.Should().NotBe(0);
        outBuf.Offset.Should().Be(sample.Length);

        decompressBuffer.Take(outBuf.Offset).Should().Equal(sample);
      }
    }

  }

}
