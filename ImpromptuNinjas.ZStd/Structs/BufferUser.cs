using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public delegate void BufferUser(ref Buffer buf);

  [PublicAPI]
  public delegate void BufferPairUser(ref Buffer buf1, ref Buffer buf2);

}
