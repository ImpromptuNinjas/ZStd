using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public enum Strategy {

    Fast = 1,

    DFast = 2,

    Greedy = 3,

    Lazy = 4,

    Lazy2 = 5,

    BtLazy2 = 6,

    BtOpt = 7,

    BtUltra = 8,

    BtUltra2 = 9

  }

}
