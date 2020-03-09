using System;

namespace ImpromptuNinjas.ZStd.Utilities {

  public sealed partial class MemoryRegionStream {

    private static void ValidateReadWriteArgs(byte[] array, int offset, int count) {
      if (array == null)
        throw new ArgumentNullException(nameof(array));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof(offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof(count));
      if (array.Length - offset < count)
        throw new ArgumentException("Invalid offset or length.");
    }

    private void ValidateDisposed() {
      if (_disposed)
        throw new ObjectDisposedException(GetType().Name);
    }

    private static unsafe void ValidateSpan(ReadOnlySpan<byte> span, string argName) {
      if (AsPointer(span) == null)
        throw new ArgumentNullException(argName);
      if (span.Length < 0)
        throw new ArgumentOutOfRangeException(argName);
    }

  }

}
