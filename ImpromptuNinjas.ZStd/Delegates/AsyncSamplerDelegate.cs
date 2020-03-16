using System;
using System.Collections.Generic;

namespace ImpromptuNinjas.ZStd {

  #if !NETSTANDARD
  public delegate IAsyncEnumerable<ArraySegment<byte>> AsyncSamplerDelegate();

  #endif
}
