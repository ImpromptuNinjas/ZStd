using System;
using System.Collections.Generic;

namespace ImpromptuNinjas.ZStd {

  #if NETSTANDARD2_1 || NETCOREAPP
  public delegate IAsyncEnumerable<ArraySegment<byte>> AsyncSamplerDelegate();

  #endif
}
