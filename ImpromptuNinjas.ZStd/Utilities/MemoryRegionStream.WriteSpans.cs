using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace ImpromptuNinjas.ZStd.Utilities {

  public sealed partial class MemoryRegionStream {

    public
#if NETSTANDARD2_1 || NETCOREAPP
    override
#endif
      unsafe void Write(ReadOnlySpan<byte> buffer) {
      ValidateDisposed();
      ValidateSpan(buffer, nameof(buffer));
      var count = buffer.Length;
      if (count == 0)
        return;

      var maxCount = _capacity - _offset;

      if (count > maxCount)
        throw new IOException("Not enough capacity to consume write.");

      fixed (byte* pBuf = buffer)
        Unsafe.CopyBlock(_pointer + _offset, pBuf, (uint) count);

      _offset += count;

      _length = Math.Max(_length, _offset);
    }

  }

}
