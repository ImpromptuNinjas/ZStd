using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ImpromptuNinjas.ZStd.Tests {

  public static class Utilities {

    static Utilities() {
      //var min = Native.ZStd.GetMinCompressionLevel(); // will be negative ffs
      var max = Native.ZStd.GetMaxCompressionLevel();
      // 0 is 'automatic' in most cases
      CompressionLevels = Enumerable.Range(0, max+1).ToArray();
    }

    public static int[] CompressionLevels;

    public static byte[] GenerateSample()
      => Encoding.ASCII
        .GetBytes($"['a': 'constant_field', 'b': '{Random.Next()}', 'c': {Random.Next()}, 'd': '{(Random.Next(1) == 1 ? "almost" : "sometimes")} constant field']");

    public static IEnumerable<ArraySegment<byte>> GenerateSamples(int qty) {
      for (var i = 0; i < qty; ++i)
        yield return GenerateSample();
    }

    public static byte[] GenerateSampleBuffer(int samples)
      => Enumerable.Range(0, samples)
        .SelectMany(_ => GenerateSample())
        .ToArray();

    public static readonly Random Random = new Random(68473417);


    public static ZStdDictionaryBuilder CreateDictionaryFromSamples(int bufferSize, int sampleCount, int compressionLevel) {
      var dict = new ZStdDictionaryBuilder(bufferSize);

      var trainedParams = dict.Train(() => GenerateSamples(sampleCount), compressionLevel);

      Assert.NotZero(trainedParams.SegmentSize);

      Assert.NotZero(dict.Size.ToUInt64());

      Console.WriteLine($"Allocated {bufferSize / 1024.0:F1}kB for dictionary, samples: {sampleCount}, trained size: {dict.Size.ToUInt64() / 1024.0:F1}kB");
      return dict;
    }
  }

}
