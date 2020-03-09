using System;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [Flags]
  [PublicAPI]
  public enum ResetDirective {

    SessionOnly = 1,

    Parameters = 2,

    SessionAndParameters = 3

  }

}
