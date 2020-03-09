using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using ImpromptuNinjas.ZStd.Utilities;

namespace ImpromptuNinjas.ZStd.Tests {

  [TestFixture]
  public unsafe partial class StreamTests {

    [Test]
    public void PiecemealStreamRoundTrip(
      [ValueSource(typeof(Utilities), nameof(Utilities.CompressionLevels))]
      int compressionLevel,
      [Values(false, true)] bool forcedFlushing,
      [Values(false, true)] bool useDictionary
    ) {
      var dict = useDictionary
        ? Utilities.CreateDictionaryFromSamples(32 * 1024, 30, compressionLevel)
        : (ZStdDictionaryBuilder?) null;

      var sample = Utilities.GenerateSampleBuffer(1000);

      Console.WriteLine($"Compression Level: {compressionLevel}");

      var bufSize = (int) CCtx.GetUpperBound((UIntPtr) sample.Length);
      var buffer = new byte[bufSize]; // compression is much worse when fed piecemeal
      fixed (byte* pBuffer = &buffer[0]) {
        using var compressed = new MemoryRegionStream(pBuffer, bufSize);
        using var toDecompress = new MemoryRegionStream(pBuffer, bufSize);

        // compression

        var run = 0;
        var wroteThisRun = 0L;

        IEnumerable<long> Feed() {
          using (var compressStream = new ZStdCompressStream(compressed)) {
            compressStream.Compressor.Set(CompressionParameter.CompressionLevel, compressionLevel);
            compressStream.Compressor.UseDictionary(dict?.CreateCompressorDictionary());

            for (var i = 0; i < sample.Length;) {
              // NOTE: failures happen if fed with below 12 bytes at a time
              var x = Utilities.Random.Next(1, 12) * (1 + run);
              var rem = sample.Length - (i + x);
              if (rem < 0)
                x += rem;
              compressStream.Write(sample, i, x);
              compressStream.Flush(forcedFlushing);
              wroteThisRun = compressed.Length - toDecompress.Length;
              if (wroteThisRun > 0)
                toDecompress.SetLength(compressed.Length);

              i += x;
              yield return wroteThisRun;
            }
          }

          wroteThisRun = compressed.Length - toDecompress.Length;
          if (wroteThisRun > 0)
            toDecompress.SetLength(compressed.Length);

          Console.WriteLine($"Compressed to: {compressed.Length} ({(double) compressed.Length / (double) sample.Length:P})");

          yield return wroteThisRun;
        }

        // decompression

        compressed.Position = 0;

        using var decompressStream = new ZStdDecompressStream(toDecompress);

        decompressStream.Decompressor.UseDictionary(dict?.CreateDecompressorDictionary());

        var decompressed = new byte[sample.Length];

        using var feed = Feed().GetEnumerator();
        var totalRead = 0;
        do {
          int read;
          var readThisRun = 0;
          do {
            read = decompressStream.Read(decompressed, totalRead, decompressed.Length - totalRead);
            CollectionAssert.AreEqual(sample.Skip(totalRead).Take(read), decompressed.Skip(totalRead).Take(read));
            totalRead += read;
            readThisRun += read;
          } while (read > 0);

          if (wroteThisRun != readThisRun)
            Debugger.Break();
          Console.WriteLine($"Run: {run++} Wrote: {wroteThisRun}, Read: {readThisRun}");
        } while (feed.MoveNext());

        Assert.AreEqual(-1, decompressStream.ReadByte());

        CollectionAssert.AreEqual(sample, decompressed);
      }
    }

  }

}
