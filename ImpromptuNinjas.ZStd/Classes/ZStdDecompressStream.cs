using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public sealed class ZStdDecompressStream : ZStdStream {

    public readonly ZStdDecompressor Decompressor;

    private int _bufferSize;

    private UIntPtr _suggestedNextInput = default;

    public unsafe ZStdDecompressStream(Stream baseStream, int bufferSize = 4096) : base(baseStream) {
      if (baseStream == null)
        throw new ArgumentNullException(nameof(baseStream));

      if (bufferSize <= 0)
        bufferSize = 4096;

      _bufferSize = bufferSize;

      Decompressor = new ZStdDecompressor();
      Input = new Buffer((byte*) Marshal.AllocHGlobal(bufferSize), (UIntPtr) bufferSize, (UIntPtr) bufferSize);
    }

    public override bool CanRead
      => true;

    public override bool CanSeek
      => false;

    public override bool CanWrite
      => false;

    public override long Length
      => throw new NotSupportedException();

    public override long Position {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    protected override unsafe void Dispose(bool disposing) {
      Flush();

      if (disposing && !Disposed) {
        Decompressor?.Dispose();
        Marshal.FreeHGlobal((IntPtr) Input.GetPinnedPointer());
        Disposed = true;
      }

      base.Dispose(disposing);
    }

    public override void Flush()
      => BaseStream.Flush();

    public override async Task FlushAsync(CancellationToken cancellationToken)
      => await BaseStream.FlushAsync(cancellationToken);

    public override unsafe int Read(byte[] buffer, int offset, int count) {
      ValidateReadWriteArgs(buffer, offset, count);
      if (count == 0 || buffer.Length == offset)
        return 0;

      fixed (byte* pBuf = &buffer[offset]) {
        Output = new Buffer(pBuf, (UIntPtr) count, default);

        var edgeOfInput = false;
        do {
          var lastPos = Output.Position;
          if (Input.Position == Input.Size && Input.Size != default) {
            Input = new Buffer(
              Input.GetPinnedPointer(),
              (UIntPtr) BaseStream.Read(new Span<byte>(Input.GetPinnedPointer(), _bufferSize)),
              default
            );
            // last read was complete and there is no more to read
            edgeOfInput = Input.Size == default;
          }

          if (Input.Size.GreaterThanOrEqualTo(_suggestedNextInput) || Input.Size.EqualTo(_bufferSize)) {
            _suggestedNextInput = Decompressor.StreamDecompress(ref Output, ref Input);
          }
          else {
            edgeOfInput = true;
          }

          // remaining might be 9 or something if the decompressor is expecting a new frame
          if (Output.Position == default && edgeOfInput)
            break;

          if (_suggestedNextInput == default && Input.Position != Input.Size)
            break;

          // break on stalls
          if (Output.Position == lastPos && _suggestedNextInput != default && (Input.Size == default || Input.Position != Input.Size))
            break;
        } while (Output.Position != Output.Size);

        if (Input.Position == Input.Size && Input.Size == default)
          Input = new Buffer(Input.GetPinnedPointer(), (UIntPtr) _bufferSize, (UIntPtr) _bufferSize);

        return (int) Output.Position;
      }
    }

    public override long Seek(long offset, SeekOrigin origin)
      => throw new NotSupportedException();

    public override void SetLength(long value)
      => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count)
      => throw new NotSupportedException();

  }

}
