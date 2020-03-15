#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
#endif
using System;
using System.Runtime.CompilerServices;
using FluentAssertions;

namespace ImpromptuNinjas.ZStd.Tests {

#if MSTEST
  [TestClass]
#else
  [TestFixture]
#endif
  public partial class BufferTests {

#if MSTEST
    [TestMethod,UseParameterValues]
#else
    [Test]
#endif
    public unsafe void Spans() {
      // ReSharper disable once RedundantCast
      var bytes = (Span<byte>) stackalloc byte[8];

      fixed (byte* pBytes = bytes) {
        var buf = new Buffer(pBytes, (UIntPtr) bytes.Length, (UIntPtr) 4);

        var complete = buf.GetCompleteSpan();

        complete.Length.Should().Be(8);

        ((IntPtr) Unsafe.AsPointer(ref complete.GetPinnableReference()))
          .Should().Be((IntPtr) Unsafe.AsPointer(ref bytes.GetPinnableReference()));

        var used = buf.GetUsedSpan();

        used.Length.Should().Be(4);

        ((IntPtr) Unsafe.AsPointer(ref used.GetPinnableReference()))
          .Should().Be((IntPtr) Unsafe.AsPointer(ref bytes.GetPinnableReference()));

        var remaining = buf.GetRemainingSpan();

        remaining.Length.Should().Be(4);

        ((IntPtr) Unsafe.AsPointer(ref remaining.GetPinnableReference()))
          .Should().Be((IntPtr) Unsafe.AsPointer(ref bytes.GetPinnableReference()) + 4);

      }
    }

  }

}
