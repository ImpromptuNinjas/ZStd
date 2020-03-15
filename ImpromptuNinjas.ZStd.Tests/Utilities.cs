using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;

namespace ImpromptuNinjas.ZStd.Tests {

  public static class Utilities {

    private static readonly Lazy<int[]> LazyCompressionLevels = new Lazy<int[]>(() => {
      var min = Native.ZStd.GetMinCompressionLevel(); // negative
      var max = Native.ZStd.GetMaxCompressionLevel();
      // 0 is 'automatic' in most cases
      return new[] {min, -3, -1, 0, 1, 3, max};
    });

    public static int[] CompressionLevels => LazyCompressionLevels.Value;

    public static byte[] GenerateSample()
      => Encoding.ASCII
        .GetBytes($"['a': 'constant_field', 'b': '{Random.Next()}', 'c': {Random.Next()}, 'd': '{(Random.Next(1) == 1 ? "almost" : "sometimes")} constant field']");

    public static IEnumerable<ArraySegment<byte>> GenerateSamples(int qty) {
      for (var i = 0; i < qty; ++i) {
        yield return new ArraySegment<byte>(GenerateSample());
      }
    }

    public static byte[] GenerateSampleBuffer(int samples)
      => Enumerable.Range(0, samples)
        .SelectMany(_ => GenerateSample())
        .ToArray();

    public static readonly Random Random = new Random(68473417);

    public static ZStdDictionaryBuilder CreateDictionaryFromSamples(int bufferSize, int sampleCount, int compressionLevel) {
      var dict = new ZStdDictionaryBuilder(bufferSize);

      var trainedParams = dict.Train(() => GenerateSamples(sampleCount), compressionLevel);

      trainedParams.SegmentSize.Should().NotBe(0);

      dict.Size.ToUInt64().Should().NotBe(0);

      Console.WriteLine($"Allocated {bufferSize / 1024.0:F1}kB for dictionary, samples: {sampleCount}, trained size: {dict.Size.ToUInt64() / 1024.0:F1}kB");
      return dict;
    }

  }

}
