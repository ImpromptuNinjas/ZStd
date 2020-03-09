using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  [StructLayout(LayoutKind.Sequential)]
  public readonly struct ParameterBounds {

    public readonly UIntPtr Error;

    public readonly int LowerBound;

    public readonly int UpperBound;

  }

}
