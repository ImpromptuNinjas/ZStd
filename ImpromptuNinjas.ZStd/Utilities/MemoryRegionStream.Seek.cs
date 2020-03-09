using System;
using System.IO;

namespace ImpromptuNinjas.ZStd.Utilities {

  public sealed partial class MemoryRegionStream {

    public override bool CanSeek
      => true;

    public override long Position {
      get => _offset;
      set => Seek(value, SeekOrigin.Begin);
    }

    public override long Seek(long offset, SeekOrigin origin) {
      ValidateDisposed();
      return origin switch {
        SeekOrigin.Begin => _offset = (int) offset,
        SeekOrigin.Current => _offset = (int) offset + _offset,
        SeekOrigin.End => _offset = (int) offset + _length,
        _ => throw new ArgumentOutOfRangeException(nameof(origin))
      };
    }

  }

}
