using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using ImpromptuNinjas.ZStd.Utilities;
#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
#endif

namespace ImpromptuNinjas.ZStd.Tests {

  public unsafe partial class StreamTests {

#if MSTEST
    [TestMethod,UseParameterValues]
#else
    [Test]
#endif
    public void PiecemealStreamRoundTrip(
      [ValueSource(typeof(Utilities), nameof(Utilities.CompressionLevels))]
      int compressionLevel,
      [Values(0, 1, 2)] int flushMode,
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

      // lol "min" compression level
      var bufSize = (int) (CCtx.GetUpperBound((UIntPtr) sample.Length).ToUInt32() * 1.05);
      var buffer = new byte[bufSize]; // compression is much worse when fed piecemeal
      fixed (byte* pBuffer = &buffer[0]) {
        using var compressed = new MemoryRegionStream(pBuffer, bufSize);
        using var toDecompress = new MemoryRegionStream(pBuffer, bufSize);

        // compression

        var run = 0;
        var expected = 0L;
        var totalWrote = 0L;
        var wroteThisRun = 0L;

        IEnumerable<long> Feed() {
          using (var cDict = dict?.CreateCompressorDictionary())
          using (var compressStream = new ZStdCompressStream(compressed)) {
            compressStream.Compressor.Set(CompressionParameter.CompressionLevel, compressionLevel);
            compressStream.Compressor.UseDictionary(cDict);

            for (var i = 0; i < sample.Length;) {
              // NOTE: failures happen if fed with below 12 bytes at a time
              var x = Utilities.Random.Next(1, 12) * (1 + run);
              var rem = sample.Length - (i + x);
              if (rem < 0)
                x += rem;
              expected += x;
              compressStream.Write(sample, i, x);
              if (flushMode != 0)
                compressStream.Flush(flushMode == 2);
              wroteThisRun = compressed.Length - toDecompress.Length;
              if (wroteThisRun > 0)
                toDecompress.SetLength(compressed.Length);
              totalWrote += wroteThisRun;

              i += x;
              yield return wroteThisRun;
            }
          }

          wroteThisRun = compressed.Length - toDecompress.Length;
          if (wroteThisRun > 0)
            toDecompress.SetLength(compressed.Length);
          totalWrote += wroteThisRun;

          Console.WriteLine($"Compressed to: {compressed.Length} ({(double) compressed.Length / (double) sample.Length:P})");

          yield return wroteThisRun;
        }

        // decompression

        compressed.Position = 0;

        using var dDict = dict?.CreateDecompressorDictionary();
        using var decompressStream = new ZStdDecompressStream(toDecompress);

        decompressStream.Decompressor.UseDictionary(dDict);

        var decompressed = new byte[sample.Length];

        using var feed = Feed().GetEnumerator();
        var totalRead = 0;
        do {
          int read;
          var readThisRun = 0;
          do {
            read = decompressStream.Read(decompressed, totalRead, decompressed.Length - totalRead);
            decompressed.Skip(totalRead).Take(read).Should().Equal(sample.Skip(totalRead).Take(read));
            totalRead += read;
            readThisRun += read;
          } while (read > 0);

          //if (wroteThisRun != readThisRun)
          //  Debugger.Break();
          Console.WriteLine($"Run: {run++} Wrote: {wroteThisRun}, Read: {readThisRun}");
        } while (feed.MoveNext());
        Console.WriteLine($"Total Wrote: {totalWrote}, Read: {totalRead}/{expected}");

        decompressStream.ReadByte().Should().Be(-1);

        decompressed.Should().Equal(sample);
      }
    }

  }

}
