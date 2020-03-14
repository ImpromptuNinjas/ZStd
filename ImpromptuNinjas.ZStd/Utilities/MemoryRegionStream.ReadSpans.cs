using System;
using System.Runtime.CompilerServices;

namespace ImpromptuNinjas.ZStd.Utilities {

  public sealed partial class MemoryRegionStream {

    public
#if !NETSTANDARD
    override
#endif
      unsafe int Read(Span<byte> buffer) {
      ValidateDisposed();
      ValidateSpan(buffer, nameof(buffer));
      var count = buffer.Length;
      if (count == 0)
        return 0;

      var maxCount = _length - _offset;

      if (maxCount == 0)
        return 0;

      var read = Math.Min(count, maxCount);

      fixed (byte* pBuf = buffer)
        Unsafe.CopyBlock(pBuf, _pointer + _offset, (uint) read);

      _offset += read;

      return read;
    }

  }

}
