using System;
using System.Runtime.CompilerServices;

namespace ImpromptuNinjas.ZStd {

  public partial struct ZStdDictionaryBuilder {

    public DictionaryTrainingParameters Train(SamplerDelegate sampler, int compressionLevel = default, uint nbThreads = 1, uint tuningSteps = 0) {
      var parameters = GetDefaultTrainingParameters(compressionLevel, nbThreads, tuningSteps);

      Train(sampler, ref parameters, compressionLevel, nbThreads, tuningSteps);

      return parameters;
    }

    public DictionaryTrainingParameters Train(AsyncSamplerDelegate sampler, int compressionLevel = default, uint nbThreads = 1, uint tuningSteps = 0) {
      var parameters = GetDefaultTrainingParameters(compressionLevel, nbThreads, tuningSteps);

      Train(sampler, ref parameters, compressionLevel, nbThreads, tuningSteps);

      return parameters;
    }

    public void Train(SamplerDelegate sampler, ref DictionaryTrainingParameters parameters, int compressionLevel = default, uint nbThreads = 1, uint tuningSteps = 0) {
      var (samplesBuffer, samplesSizes) = GatherSamples(sampler);

      Size = Native.ZDict.Train(
          Data,
          samplesBuffer, (ReadOnlySpan<UIntPtr>) samplesSizes,
          ref parameters
        )
        .EnsureZDictSuccess();
    }

    public unsafe void Train(AsyncSamplerDelegate sampler, ref DictionaryTrainingParameters parameters, int compressionLevel = default, uint nbThreads = 1, uint tuningSteps = 0) {
      var data = Data;

      fixed (DictionaryTrainingParameters* pParameters = &parameters) {
        Size = GatherSamples(sampler)
          .ContinueWith((samplesTask, state) => {
            // ReSharper disable once PossibleNullReferenceException
            // ReSharper disable once VariableHidesOuterVariable
            var pParameters = (DictionaryTrainingParameters*) (IntPtr) state;
            var (samplesBuffer, samplesSizes) = samplesTask.GetAwaiter().GetResult();
            return Native.ZDict.Train(
              data,
              samplesBuffer, (ReadOnlySpan<UIntPtr>) samplesSizes,
              ref Unsafe.AsRef<DictionaryTrainingParameters>(pParameters)
            );
          }, (IntPtr) pParameters)
          .GetAwaiter().GetResult()
          .EnsureZDictSuccess();
      }
    }

  }

}
