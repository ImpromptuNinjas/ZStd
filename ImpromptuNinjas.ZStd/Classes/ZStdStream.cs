using System;
using System.IO;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public abstract class ZStdStream : Stream {

    public readonly Stream BaseStream;

    protected Buffer Input;

    protected Buffer Output;

    protected bool Disposed;

    protected ZStdStream(Stream baseStream)
      => BaseStream = baseStream;


    protected void ValidateReadWriteArgs(byte[] array, int offset, int count)
    {
      if (Disposed)
        throw new ObjectDisposedException(GetType().Name);
      if (array == null)
        throw new ArgumentNullException(nameof(array));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof(offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof(count));
      if (array.Length - offset < count)
        throw new ArgumentException("Invalid offset or length.");
    }

  }

}
