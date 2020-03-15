using System;
using System.IO;
using FluentAssertions;
#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
#endif

namespace ImpromptuNinjas.ZStd.Tests {

  public partial class StreamTests {

#if MSTEST
    [TestMethod,UseParameterValues]
#else
    [Test]
#endif
    public void StreamRoundTrip(
      [ValueSource(typeof(Utilities), nameof(Utilities.CompressionLevels))]
      int compressionLevel,
      [Values(0, 1, 2)] int flushMode,
      [Values(false, true)] bool useDictionary
    ) {
      var dict = useDictionary
        ? Utilities.CreateDictionaryFromSamples(32 * 1024, 30, compressionLevel)
        : (ZStdDictionaryBuilder?) null;

      var sample = Utilities.GenerateSampleBuffer(1000);

      Console.WriteLine($"Compression Level: {compressionLevel}");

      using var compressed = new MemoryStream();

      // compression

      using (var cDict = dict?.CreateCompressorDictionary(compressionLevel))
      using (var compressStream = new ZStdCompressStream(compressed)) {
        compressStream.Compressor.UseDictionary(cDict);

        compressStream.Compressor.Set(CompressionParameter.CompressionLevel, compressionLevel);

        compressStream.Write(sample, 0, sample.Length);

        if (flushMode != 0)
          compressStream.Flush(flushMode == 2);
      }

      Console.WriteLine($"Compressed to: {compressed.Length} ({(double) compressed.Length / (double) sample.Length:P})");

      // decompression

      compressed.Position = 0;

      using var dDict = dict?.CreateDecompressorDictionary();
      using var decompressStream = new ZStdDecompressStream(compressed);

      decompressStream.Decompressor.UseDictionary(dDict);

      var decompressed = new byte[sample.Length];

      decompressStream.Read(decompressed, 0, decompressed.Length);

      decompressStream.ReadByte().Should().Be(-1);

      decompressed.Should().Equal(sample);
    }

  }

}
