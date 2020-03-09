using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public enum DecompressionParameter {

    /* Select a size limit (in power of 2) beyond which
     * the streaming API will refuse to allocate memory buffer
     * in order to protect the host from unreasonable memory requirements.
     * This parameter is only useful in streaming mode, since no internal buffer is allocated in single-pass mode.
     * By default, a decompression context accepts window sizes <= (1 << ZSTD_WINDOWLOG_LIMIT_DEFAULT).
     * Special: value 0 means "use default maximum windowLog". */
    WindowLogMax = 100,

    /* note : additional experimental parameters are also available
     * within the experimental section of the API.
     * At the time of this writing, they include :
     * ZSTD_c_format
     * Because they are not stable, it's necessary to define ZSTD_STATIC_LINKING_ONLY to access them.
     * note : never ever use experimentalParam? names directly
     */
    ExperimentalParam1 = 1000

  }

}
