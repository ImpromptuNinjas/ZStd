using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace ImpromptuNinjas.ZStd {

  public static partial class Native {

    private const string LibName = "zstd";

    internal static string LibPath;

    private static readonly unsafe Lazy<IntPtr> _lazyLoadedLib = new Lazy<IntPtr>(() => {
      var dir = Path.GetDirectoryName(new Uri(typeof(Native).Assembly.CodeBase
          ?? throw new PlatformNotSupportedException()).LocalPath)
        ?? throw new PlatformNotSupportedException();

      var ptrBits = sizeof(void*) * 8;

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        LibPath = Path.Combine(dir, "runtimes", ptrBits == 32 ? "win-x86" : "win-x64", "native", "libzstd.dll");
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        LibPath = Path.Combine(dir, "runtimes", "osx-x64", "native", "libzstd.dylib");
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
        LibPath = Path.Combine(dir, "runtimes", $"{(IsMusl() ? "linux-musl-" : "linux-")}{GetProcArchString()}", "native", "libzstd.so");
      else throw new PlatformNotSupportedException();

      var lib = NativeLibrary.Load(LibPath);

      if (lib == default)
        throw new DllNotFoundException($"Unable to load {LibPath}");

      return lib;
    }, LazyThreadSafetyMode.ExecutionAndPublication);

    internal static IntPtr Lib => _lazyLoadedLib.Value;

    static Native()
      => NativeLibrary.SetDllImportResolver(typeof(Native).Assembly,
        (name, assembly, path)
          => {
          if (name != LibName)
            return default;

          Debug.Assert(Lib != default);
          return Lib;
        });

    internal static void Init()
      => Debug.Assert(Lib != default);

  }

}
