using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  public partial class ZStdDictionaryBuilder {

#if NETSTANDARD2_1 || NETCOREAPP
    public DictionaryTrainingParameters Train(AsyncSamplerDelegate sampler, int compressionLevel = default, uint nbThreads = 1, uint tuningSteps = 0) {
      var parameters = GetDefaultTrainingParameters(compressionLevel, nbThreads, tuningSteps);

      Train(sampler, ref parameters, compressionLevel, nbThreads, tuningSteps);

      return parameters;
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

    [MustUseReturnValue]
    private static async Task<(ArraySegment<byte> Samples, UIntPtr[] SamplesSizes)> GatherSamples(AsyncSamplerDelegate sampler) {
      await using var stream = RecyclableMemoryStreamManager.GetStream("ZStd Dictionary Sampling Buffer");
      var sizes = new LinkedList<UIntPtr>();

      await foreach (var sample in sampler()) {
        if (sample.Array == null)
          continue;

        stream.Write(sample.Array, sample.Offset, sample.Count);
        sizes.AddLast((UIntPtr) (sample.Count - sample.Offset));
      }

      var sizesArray = new UIntPtr[sizes.Count];
      sizes.CopyTo(sizesArray, 0);

      return (new ArraySegment<byte>(stream.GetBuffer(), 0, (int) stream.Length), sizesArray);
    }

#endif

  }

}
