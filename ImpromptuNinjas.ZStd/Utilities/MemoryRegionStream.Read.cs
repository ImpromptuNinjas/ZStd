using System;
using System.Runtime.CompilerServices;

namespace ImpromptuNinjas.ZStd.Utilities {

  public sealed partial class MemoryRegionStream {

    public override bool CanRead
      => _offset < _length;

    public override unsafe int ReadByte() {
      ValidateDisposed();
      if (_length - _offset < 1)
        return -1;

      return *(_pointer + _offset++);
    }

    public override unsafe int Read(byte[] buffer, int offset, int count) {
      ValidateDisposed();
      ValidateReadWriteArgs(buffer, offset, count);
      if (count == 0 || buffer.Length == offset)
        return 0;

      var maxCount = _length - _offset;

      if (maxCount == 0)
        return 0;

      var read = Math.Min(count, maxCount);

      fixed (byte* pBuf = &buffer[offset])
        Unsafe.CopyBlock(pBuf, _pointer + _offset, (uint) read);

      _offset += read;

      return read;
    }

  }

}
