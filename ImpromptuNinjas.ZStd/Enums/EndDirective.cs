using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public enum EndDirective {

    /* collect more data, encoder decides when to output compressed result, for optimal compression ratio */
    Continue = 0,

    /* flush any data provided so far,
     * it creates (at least) one new block, that can be decoded immediately on reception;
     * frame will continue: any future data can still reference previously compressed data, improving compression.
     * note : multithreaded compression will block to flush as much output as possible. */
    Flush = 1,

    /* flush any data provided so far,
     * it creates (at least) one new block, that can be decoded immediately on reception;
     * frame will continue: any future data can still reference previously compressed data, improving compression.
     * note : multithreaded compression will block to flush as much output as possible. */
    End = 2

  }

}
