using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  public static partial class Extensions {

#if NETSTANDARD1_1
    internal static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new() {
      if (dictionary.TryGetValue(key, out var value))
        return value;

      return dictionary[key] = new TValue();
    }
#endif

    internal static string GetLocalCodeBaseDirectory(this Assembly asm)
#if NETSTANDARD1_1 || NETSTANDARD1_4
      => Path.GetDirectoryName(new Uri((asm.ManifestModule?.FullyQualifiedName
          ?? throw new PlatformNotSupportedException()).Replace("#", "%23")).LocalPath)
        ?? throw new PlatformNotSupportedException();
#else
      => Path.GetDirectoryName(new Uri((asm.CodeBase
            ?? throw new PlatformNotSupportedException())
          .Replace("#", "%23")).LocalPath)
        ?? throw new PlatformNotSupportedException();
#endif

#if NETSTANDARD1_1 || NETSTANDARD1_4
    internal static Assembly GetAssembly(this Type type)
      => type.GetTypeInfo().Assembly;
#else
    internal static Assembly GetAssembly(this Type type)
      => type.Assembly;
#endif

    internal static UIntPtr EnsureZStdSuccess(this UIntPtr value) {
      if (Native.ZDict.IsError(value) != 0)
        ThrowException(value, Native.ZStd.GetErrorName(value));
      return value;
    }

    internal static UIntPtr EnsureZDictSuccess(this UIntPtr value) {
      if (Native.ZDict.IsError(value) != 0)
        ThrowException(value, Native.ZDict.GetErrorName(value));
      return value;
    }

    private static void ThrowException(UIntPtr value, string message) {
      var code = unchecked(0 - (uint) (ulong) value);
      throw new ZStdException(message, code);
    }

    internal static unsafe int CompareTo(this UIntPtr a, UIntPtr b)
      => sizeof(UIntPtr) == 8
        ? a.ToUInt64().CompareTo(b.ToUInt64())
        : a.ToUInt32().CompareTo(b.ToUInt32());

    internal static bool GreaterThan(this UIntPtr a, UIntPtr b)
      => a.CompareTo(b) > 0;

    internal static bool LessThan(this UIntPtr a, UIntPtr b)
      => a.CompareTo(b) < 0;

    internal static bool GreaterThanOrEqualTo(this UIntPtr a, UIntPtr b)
      => a.CompareTo(b) >= 0;

    internal static bool LessThanOrEqualTo(this UIntPtr a, UIntPtr b)
      => a.CompareTo(b) <= 0;

    internal static unsafe bool EqualTo(this UIntPtr a, int b)
      => sizeof(UIntPtr) == 8
        ? checked((long) a.ToUInt64()) == b
        : a.ToUInt32() == b;

    internal static unsafe bool EqualTo(this UIntPtr a, long b)
      => sizeof(UIntPtr) == 8
        ? checked((long) a.ToUInt64()) == b
        : a.ToUInt32() == b;

#if NETSTANDARD
    internal static void Write(this Stream stream, ReadOnlySpan<byte> bytes) {
      var count = bytes.Length;
      var copy = ArrayPool<byte>.Shared.Rent(count);
      try {
        bytes.CopyTo(copy);
        stream.Write(copy, 0, count);
      }
      finally {
        ArrayPool<byte>.Shared.Return(copy);
      }
    }

    internal static int Read(this Stream stream, Span<byte> bytes) {
      var count = bytes.Length;
      var copy = ArrayPool<byte>.Shared.Rent(count);
      try {
        var read = stream.Read(copy, 0, count);
        copy.CopyTo(bytes);

        return read;
      }
      finally {
        ArrayPool<byte>.Shared.Return(copy);
      }
    }
#endif

  }

}
