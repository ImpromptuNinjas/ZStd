using System;
using System.IO;
using NUnit.Framework;

namespace ImpromptuNinjas.ZStd.Tests {

  public partial class StreamTests {

    [Test]
    public void StreamRoundTrip(
      [ValueSource(typeof(Utilities), nameof(Utilities.CompressionLevels))]
      int compressionLevel,
      [Values(false, true)] bool forceFlush,
      [Values(false, true)] bool useDictionary
    ) {
      var dict = useDictionary
        ? Utilities.CreateDictionaryFromSamples(32 * 1024, 30, compressionLevel)
        : (ZStdDictionaryBuilder?) null;

      var sample = Utilities.GenerateSampleBuffer(1000);

      Console.WriteLine($"Compression Level: {compressionLevel}");

      using var compressed = new MemoryStream();

      // compression

      using (var compressStream = new ZStdCompressStream(compressed)) {
        using var cDict = dict?.CreateCompressorDictionary(compressionLevel);

        compressStream.Compressor.UseDictionary(cDict);

        compressStream.Compressor.Set(CompressionParameter.CompressionLevel, compressionLevel);

        compressStream.Write(sample);

        compressStream.Flush(forceFlush);
      }

      Console.WriteLine($"Compressed to: {compressed.Length} ({(double) compressed.Length / (double) sample.Length:P})");

      // decompression

      compressed.Position = 0;

      using var decompressStream = new ZStdDecompressStream(compressed);

      using var dDict = dict?.CreateDecompressorDictionary();

      decompressStream.Decompressor.UseDictionary(dDict);

      var decompressed = new byte[sample.Length];

      decompressStream.Read(decompressed, 0, decompressed.Length);

      Assert.AreEqual(-1, decompressStream.ReadByte());

      CollectionAssert.AreEqual(sample, decompressed);
    }

  }

}
