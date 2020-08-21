using System;
using System.Collections.Generic;

namespace ImpromptuNinjas.ZStd {

  public partial class ZStdDictionaryBuilder {

    public DictionaryTrainingParameters Train(SamplerDelegate sampler, int compressionLevel = default, uint nbThreads = 1, uint tuningSteps = 0) {
      var parameters = GetDefaultTrainingParameters(compressionLevel, nbThreads, tuningSteps);

      Train(sampler, ref parameters);

      return parameters;
    }

    public void Finalize(ReadOnlySpan<byte> contents, SamplerDelegate sampler, ref DictionaryParameters parameters) {
      var samplesBuffer = GatherSamples(sampler, out var samplesSizes);
      Size = Native.ZDict.Finalize(
          Data,
          contents,
          samplesBuffer,
          (ReadOnlySpan<UIntPtr>) samplesSizes,
          ref parameters
        )
        .EnsureZDictSuccess();
    }

    public void Train(SamplerDelegate sampler, ref DictionaryTrainingParameters parameters) {
      var samplesBuffer = GatherSamples(sampler, out var samplesSizes);

      Size = Native.ZDict.Train(
          Data,
          samplesBuffer, (ReadOnlySpan<UIntPtr>) samplesSizes,
          ref parameters
        )
        .EnsureZDictSuccess();
    }

    public ZStdDictionaryTrainer CreateTrainer(int compressionLevel = default, uint nbThreads = 1, uint tuningSteps = 0)
      => new ZStdDictionaryTrainer(this, GetDefaultTrainingParameters(compressionLevel, nbThreads, tuningSteps));

  }

}
