using System;
using System.Runtime.CompilerServices;

namespace ImpromptuNinjas.ZStd {

  public partial class ZStdDictionaryBuilder {

    public DictionaryTrainingParameters Train(SamplerDelegate sampler, int compressionLevel = default, uint nbThreads = 1, uint tuningSteps = 0) {
      var parameters = GetDefaultTrainingParameters(compressionLevel, nbThreads, tuningSteps);

      Train(sampler, ref parameters, compressionLevel, nbThreads, tuningSteps);

      return parameters;
    }

    public void Train(SamplerDelegate sampler, ref DictionaryTrainingParameters parameters, int compressionLevel = default, uint nbThreads = 1, uint tuningSteps = 0) {
      var samplesBuffer = GatherSamples(sampler, out var samplesSizes);

      Size = Native.ZDict.Train(
          Data,
          samplesBuffer, (ReadOnlySpan<UIntPtr>) samplesSizes,
          ref parameters
        )
        .EnsureZDictSuccess();
    }

  }

}
