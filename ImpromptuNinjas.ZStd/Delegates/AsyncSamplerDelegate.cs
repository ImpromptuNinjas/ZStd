using System;
using System.Collections.Generic;

namespace ImpromptuNinjas.ZStd {

  #if !NETSTANDARD || NETSTANDARD2_1
  public delegate IAsyncEnumerable<ArraySegment<byte>> AsyncSamplerDelegate();

  #endif
}
