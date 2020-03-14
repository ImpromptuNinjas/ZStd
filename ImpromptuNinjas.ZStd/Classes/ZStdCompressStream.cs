using System;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public sealed unsafe class ZStdCompressStream : ZStdStream {

    public readonly ZStdCompressor Compressor;

    private UIntPtr _needsFlushing;

    public ZStdCompressStream(Stream baseStream, int bufferSize = 4096) : base(baseStream) {
      if (baseStream == null)
        throw new ArgumentNullException(nameof(baseStream));

      if (bufferSize <= 0)
        bufferSize = 4096;
      Compressor = new ZStdCompressor();
      Output = new Buffer((byte*) Marshal.AllocHGlobal(bufferSize), (UIntPtr) bufferSize, default);
    }

    public override bool CanRead
      => false;

    public override bool CanSeek
      => false;

    public override bool CanWrite
      => true;

    public override long Length
      => throw new NotSupportedException();

    public override long Position {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    public bool ForceFlushing { get; set; } = false;

    protected override void Dispose(bool disposing) {
      Flush(true);

      if (disposing && !Disposed) {
        Compressor?.Dispose();
        Marshal.FreeHGlobal((IntPtr) Output.GetPinnedPointer());
        Disposed = true;
      }

      base.Dispose(disposing);
    }

    public override void Write(byte[] buffer, int offset, int count) {
      ValidateReadWriteArgs(buffer, offset, count);
      if (count == 0 || buffer.Length == offset)
        return;

      fixed (byte* pBuf = &buffer[offset]) {
        Input = new Buffer(pBuf, (UIntPtr) count, default);
        do {
          // buffer full, copy to output
          if (Output.Position == Output.Size) {
            BaseStream.Write(new ReadOnlySpan<byte>(Output.GetPinnedPointer(), (int) Output.Position));
            Output.Position = default;
          }

          _needsFlushing = Compressor.StreamCompress(ref Output, ref Input, EndDirective.Continue);
          if (_needsFlushing == default && Output.Position != Output.Size)
            break;
        } while (Input.Position != Input.Size && (_needsFlushing != default || Output.Position == Output.Size));

        if (Output.Position == default) {
          return;
        }

        BaseStream.Write(new ReadOnlySpan<byte>(Output.GetPinnedPointer(), (int) Output.Position));
        Output.Position = default;
      }
    }

    public override void Flush() {
      Input = new Buffer(default, default, default);

      if (ForceFlushing) {
        do {
          _needsFlushing = Compressor.StreamCompress(ref Output, ref Input, EndDirective.End);

          if (Output.Position == default)
            continue;

          BaseStream.Write(Output.GetUsedSpan());
          Output.Position = default;
        } while (_needsFlushing != default);

        BaseStream.Flush();
      }
      else {
        do {
          _needsFlushing = Compressor.StreamCompress(ref Output, ref Input, EndDirective.Flush);

          if (Output.Position == default)
            continue;

          BaseStream.Write(Output.GetUsedSpan());
          Output.Position = default;
        } while (_needsFlushing != default);

        BaseStream.Flush();
      }
    }

    public void Flush(bool force) {
      var oldForceFlushing = ForceFlushing;
      ForceFlushing = force;
      try {
        Flush();
      }
      finally {
        ForceFlushing = oldForceFlushing;
      }
    }

    public override int Read(byte[] buffer, int offset, int count)
      => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin)
      => throw new NotSupportedException();

    public override void SetLength(long value)
      => throw new NotSupportedException();

  }

}
