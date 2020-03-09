using System;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd.Utilities {

  [PublicAPI]
  public sealed partial class MemoryRegionStream : Stream {

    private readonly unsafe byte* _pointer;

    private readonly int _capacity;

    private int _length;

    private int _offset;

    private bool _disposed;

    public unsafe MemoryRegionStream(ReadOnlySpan<byte> region, int length = 0, int offset = 0) {
      _pointer = AsPointer(region);
      _capacity = region.Length;
      _length = length;
      _offset = offset;
    }

    public unsafe MemoryRegionStream(byte* pointer, int capacity, int length = 0, int offset = 0) {
      _pointer = pointer;
      _capacity = capacity;
      _length = length;
      _offset = offset;
    }

    private static unsafe byte* AsPointer(ReadOnlySpan<byte> region)
      => (byte*) Unsafe.AsPointer(ref Unsafe.AsRef(region.GetPinnableReference()));

    public override void Flush()
      => ValidateDisposed();

    public override long Length
      => _length;

    public override void SetLength(long value) {
      ValidateDisposed();
      if (value > _capacity)
        throw new ArgumentOutOfRangeException(nameof(value));

      _length = (int) value;
    }

    protected override void Dispose(bool disposing) {
      if (disposing)
        _disposed = true;

      base.Dispose(disposing);
    }

  }

}
