using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImpromptuNinjas.ZStd {

  public partial class Native {

#if NETSTANDARD

    private delegate IntPtr DllImportResolver(
      string libraryName,
      Assembly assembly,
      DllImportSearchPath? searchPath
    );

    private abstract class NativeLibrary {

      private static readonly INativeLibraryLoader Loader
        = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
          ? Kernel32.Instance
          : LibDl.Instance;

      private static readonly ConditionalWeakTable<Assembly, LinkedList<DllImportResolver>> _resolvers
        = new ConditionalWeakTable<Assembly, LinkedList<DllImportResolver>>();

      public static void SetDllImportResolver(Assembly assembly, DllImportResolver resolver) {
        lock (_resolvers)
          _resolvers.GetOrCreateValue(assembly).AddLast(resolver);
      }

      public static IntPtr GetExport(IntPtr handle, string name)
        => Loader.GetExport(handle, name);

      public static IntPtr Load(string libraryName, Assembly assembly, DllImportSearchPath? searchPath = null) {
        lock (_resolvers) {
          if (!_resolvers.TryGetValue(assembly, out var resolvers))
            return Load(libraryName);

          foreach (var resolver in resolvers) {
            var result = resolver(libraryName, assembly, searchPath);
            if (result != default)
              return result;
          }

          return Load(libraryName);
        }
      }

      public static IntPtr Load(string libraryPath)
        => Loader.Load(libraryPath);

      private interface INativeLibraryLoader {

        IntPtr Load(string libraryPath);

        IntPtr GetExport(IntPtr handle, string name);

      }

      private static class LibDl {

        static LibDl() {
          try {
            var loader = LibDl2.Instance;
            loader.GetExport(default, null);
            Instance = loader;
          }
          catch (Exception) {
            var loader = LibDl1.Instance;
            loader.GetExport(default, null);
            Instance = loader;
          }
        }

        internal static readonly INativeLibraryLoader Instance;

      }

      private sealed class LibDl1 : INativeLibraryLoader {

        // ReSharper disable once MemberHidesStaticFromOuterClass
        private const string LibName = "libdl"; // can be libdl.so or libdl.dylib

        private LibDl1() {
        }

        internal static readonly INativeLibraryLoader Instance = new LibDl1();

        [DllImport(LibName, EntryPoint = "dlopen")]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr Load(string fileName, int flags);

        [DllImport(LibName, EntryPoint = "dlsym")]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr GetExport(IntPtr handle, string symbol);

        IntPtr INativeLibraryLoader.Load(string libraryPath)
          => Load(libraryPath, 0x0102 /*RTLD_NOW|RTLD_GLOBAL*/);

        IntPtr INativeLibraryLoader.GetExport(IntPtr handle, string name)
          => GetExport(handle, name);

      }

      private sealed class LibDl2 : INativeLibraryLoader {

        // ReSharper disable once MemberHidesStaticFromOuterClass
        private const string LibName = "libdl.so.2";

        private LibDl2() {
        }

        internal static readonly INativeLibraryLoader Instance = new LibDl2();

        [DllImport(LibName, EntryPoint = "dlopen")]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr Load(string fileName, int flags);

        [DllImport(LibName, EntryPoint = "dlsym")]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr GetExport(IntPtr handle, string symbol);

        IntPtr INativeLibraryLoader.Load(string libraryPath)
          => Load(libraryPath, 0x0102 /*RTLD_NOW|RTLD_GLOBAL*/);

        IntPtr INativeLibraryLoader.GetExport(IntPtr handle, string name)
          => GetExport(handle, name);

      }

      private sealed class Kernel32 : INativeLibraryLoader {

        // ReSharper disable once MemberHidesStaticFromOuterClass
        private const string LibName = "kernel32";

        private Kernel32() {
        }

        internal static readonly INativeLibraryLoader Instance = new Kernel32();

        [DllImport(LibName, EntryPoint = "LoadLibrary", SetLastError = true)]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr Load(string lpFileName);

        [DllImport(LibName, EntryPoint = "GetProcAddress")]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr GetExport(IntPtr handle, string procedureName);

        IntPtr INativeLibraryLoader.Load(string libraryPath)
          => Load(libraryPath);

        IntPtr INativeLibraryLoader.GetExport(IntPtr handle, string name)
          => GetExport(handle, name);

      }

    }
#endif

  }

}
