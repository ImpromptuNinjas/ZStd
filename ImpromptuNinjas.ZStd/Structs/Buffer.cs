using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  [StructLayout(LayoutKind.Sequential)]
  public struct Buffer {

    private readonly unsafe byte* _pinnedPointer;

    public readonly UIntPtr Size;

    public UIntPtr Position;

    public unsafe Buffer(byte* pinnedPointer, UIntPtr size, UIntPtr position) {
      _pinnedPointer = pinnedPointer;
      Size = size;
      Position = position;
    }

    public unsafe byte* GetPinnedPointer()
      => _pinnedPointer;

    public unsafe Span<byte> GetCompleteSpan()
      => new Span<byte>(_pinnedPointer, (int) Size);

    public unsafe Span<byte> GetRemainingSpan()
      => sizeof(void*) == 8
        ? new Span<byte>(_pinnedPointer + Position.ToUInt64(), (int) Size)
        : new Span<byte>(_pinnedPointer + Position.ToUInt32(), (int) Size);

    public unsafe Span<byte> GetUsableSpan()
      => sizeof(void*) == 8
        ? new Span<byte>(_pinnedPointer, (int) (Size.ToUInt64() - Position.ToUInt64()))
        : new Span<byte>(_pinnedPointer, (int) (Size.ToUInt32() - Position.ToUInt32()));
    public unsafe Span<byte> GetUsedSpan()
      => sizeof(void*) == 8
        ? new Span<byte>(_pinnedPointer, (int) (Position.ToUInt64()))
        : new Span<byte>(_pinnedPointer, (int) (Position.ToUInt32()));

    public static unsafe void WithArraySegment(ref ArraySegment<byte> seg, BufferUser bufUser) {
      if (seg == null)
        throw new ArgumentNullException(nameof(seg));

      if (seg.Array == null)
        throw new ArgumentNullException($"{nameof(seg)}.{nameof(seg.Array)}");

      fixed (byte* pointer = &seg.Array[0]) {
        var position = (UIntPtr) seg.Offset;
        var size = (UIntPtr) seg.Count;
        var buf = new Buffer(pointer, size, position);

        bufUser(ref buf);

        if (buf._pinnedPointer != pointer)
          throw new InvalidOperationException("Buffer's read-only Pointer was modified.");

        if (buf.Size != size)
          throw new InvalidOperationException("Buffer's read-only Size was modified.");

        if (buf.Position != position)
          seg = new ArraySegment<byte>(seg.Array, (int) buf.Position, seg.Count);
      }
    }

    public static unsafe void WithArraySegmentPair(ref ArraySegment<byte> seg1, ref ArraySegment<byte> seg2, BufferPairUser bufUser) {
      if (seg1 == null)
        throw new ArgumentNullException(nameof(seg1));
      if (seg2 == null)
        throw new ArgumentNullException(nameof(seg2));

      if (seg1.Array == null)
        throw new ArgumentNullException($"{nameof(seg1)}.{nameof(seg1.Array)}");
      if (seg2.Array == null)
        throw new ArgumentNullException($"{nameof(seg2)}.{nameof(seg2.Array)}");

      fixed (byte* pointer1 = &seg1.Array[0])
      fixed (byte* pointer2 = &seg2.Array[0]) {
        var position1 = (UIntPtr) seg1.Offset;
        var position2 = (UIntPtr) seg2.Offset;
        var size1 = (UIntPtr) (seg1.Count + seg1.Offset);
        var size2 = (UIntPtr) (seg2.Count + seg2.Offset);

        var buf1 = new Buffer(pointer1, size1, position1);
        var buf2 = new Buffer(pointer2, size2, position2);

        bufUser(ref buf1, ref buf2);

        if (buf1._pinnedPointer != pointer1)
          throw new InvalidOperationException("Buffer's read-only Pointer was modified.");
        if (buf1.Size != size1)
          throw new InvalidOperationException("Buffer's read-only Size was modified.");

        if (buf2._pinnedPointer != pointer2)
          throw new InvalidOperationException("Buffer's read-only Pointer was modified.");
        if (buf2.Size != size2)
          throw new InvalidOperationException("Buffer's read-only Size was modified.");

        if (buf1.Position != position1)
          seg1 = new ArraySegment<byte>(seg1.Array, (int) buf1.Position, (int) (buf1.Size.ToUInt64() - buf1.Position.ToUInt64()));
        if (buf2.Position != position2)
          seg2 = new ArraySegment<byte>(seg2.Array, (int) buf2.Position, (int) (buf2.Size.ToUInt64() - buf2.Position.ToUInt64()));
      }
    }

  }

}
