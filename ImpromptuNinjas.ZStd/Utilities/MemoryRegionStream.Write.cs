using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace ImpromptuNinjas.ZStd.Utilities {

  public sealed partial class MemoryRegionStream {

    public override bool CanWrite
      => _offset < _capacity;

    public override unsafe void WriteByte(byte value) {
      ValidateDisposed();
      if (_capacity - _offset < 1)
        throw new IOException("Not enough capacity to consume write.");

      *(_pointer + _offset++) = value;

      _length = Math.Max(_length, _offset);
    }

    public override unsafe void Write(byte[] buffer, int offset, int count) {
      ValidateDisposed();
      ValidateReadWriteArgs(buffer, offset, count);
      if (count == 0 || buffer.Length == offset)
        return;

      var maxCount = _capacity - _offset;

      if (count > maxCount)
        throw new IOException("Not enough capacity to consume write.");

      fixed (byte* pBuf = &buffer[offset])
        Unsafe.CopyBlock(_pointer + _offset, pBuf, (uint) count);

      _offset += count;

      _length = Math.Max(_length, _offset);
    }

  }

}
