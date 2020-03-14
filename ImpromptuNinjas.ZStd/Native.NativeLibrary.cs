using System;
using System.Collections.Generic;
using System.Diagnostics;
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

      public static IntPtr GetExport(IntPtr handle, string name) {
        if (handle == default)
          throw new ArgumentNullException(nameof(handle));
        if (string.IsNullOrEmpty(name))
          throw new ArgumentNullException(nameof(name));

        var export = Loader.GetExport(handle, name);
        if (export == default)
          throw new EntryPointNotFoundException(name);

        return export;
      }

      public static IntPtr Load(string libraryName, Assembly assembly, DllImportSearchPath? searchPath = null) {
        if (string.IsNullOrEmpty(libraryName))
          throw new ArgumentNullException(nameof(libraryName));
        if (assembly == null)
          throw new ArgumentNullException(nameof(assembly));

        lock (_resolvers) {
          if (!_resolvers.TryGetValue(assembly, out var resolvers))
            return Load(libraryName);

          foreach (var resolver in resolvers) {
            var result = resolver(libraryName, assembly, searchPath);
            if (result != default)
              return result;
          }

          var loaded = Load(libraryName);
          if (loaded == default)
            throw new DllNotFoundException(libraryName);

          return loaded;
        }
      }

      public static IntPtr Load(string libraryPath) {
        if (string.IsNullOrEmpty(libraryPath))
          throw new ArgumentNullException(nameof(libraryPath));

        var loaded = Loader.Load(libraryPath);
        if (loaded == default)
          throw new DllNotFoundException(libraryPath);

        return loaded;
      }

      private interface INativeLibraryLoader {

        void Init();

        IntPtr Load(string libraryPath);

        IntPtr GetExport(IntPtr handle, string name);

      }

      private static class LibDl {

        static LibDl() {
          Trace.TraceInformation($"LibDl initializing.");
          try {
            var loader = LibDl2.Instance;
            loader.Init();
            Instance = loader;
          }
          catch (Exception) {
            var loader = LibDl1.Instance;
            loader.Init();
            Instance = loader;
          }
        }

        internal static readonly INativeLibraryLoader Instance;

      }

      private sealed class LibDl1 : INativeLibraryLoader {

        // ReSharper disable once MemberHidesStaticFromOuterClass
        private const string LibName = "libdl"; // can be libdl.so or libdl.dylib

        private LibDl1() {
          Trace.TraceInformation($"LibDl1 initializing.");
        }

        internal static readonly INativeLibraryLoader Instance = new LibDl1();

        [DllImport(LibName, EntryPoint = "dlopen")]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr Load(string fileName, int flags);

        [DllImport(LibName, EntryPoint = "dlsym")]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr GetExport(IntPtr handle, string symbol);

        IntPtr INativeLibraryLoader.Load(string libraryPath) {
          Trace.TraceInformation($"LibDl1.Load(\"{libraryPath}\")");
          return Load(libraryPath, 0x0002 /*RTLD_NOW*/);
        }

        IntPtr INativeLibraryLoader.GetExport(IntPtr handle, string name) {
          Trace.TraceInformation($"LibDl1.GetExport({handle},\"{name}\")");
          return GetExport(handle, name);
        }

        public void Init() {
        }

      }

      private sealed class LibDl2 : INativeLibraryLoader {

        // ReSharper disable once MemberHidesStaticFromOuterClass
        private const string LibName = "libdl.so.2";

        private LibDl2() {
          Trace.TraceInformation($"LibDl2 initializing.");
        }

        internal static readonly INativeLibraryLoader Instance = new LibDl2();

        [DllImport(LibName, EntryPoint = "dlopen")]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr Load(string fileName, int flags);

        [DllImport(LibName, EntryPoint = "dlsym")]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr GetExport(IntPtr handle, string symbol);

        IntPtr INativeLibraryLoader.Load(string libraryPath) {
          Trace.TraceInformation($"LibDl2.Load(\"{libraryPath}\")");
          return Load(libraryPath, 0x0002 /*RTLD_NOW*/);
        }

        IntPtr INativeLibraryLoader.GetExport(IntPtr handle, string name) {
          Trace.TraceInformation($"LibDl2.GetExport({handle},\"{name}\")");
          return GetExport(handle, name);
        }

        public void Init() {
        }

      }

      private sealed class Kernel32 : INativeLibraryLoader {

        // ReSharper disable once MemberHidesStaticFromOuterClass
        private const string LibName = "kernel32";

        private Kernel32() {
          Trace.TraceInformation($"Kernel32 initialized.");
        }

        internal static readonly INativeLibraryLoader Instance = new Kernel32();

        [DllImport(LibName, EntryPoint = "LoadLibrary", SetLastError = true)]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr Load(string lpFileName);

        [DllImport(LibName, EntryPoint = "GetProcAddress")]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static extern IntPtr GetExport(IntPtr handle, string procedureName);

        IntPtr INativeLibraryLoader.Load(string libraryPath) {
          Trace.TraceInformation($"Kernel32.Load(\"{libraryPath}\")");
          return Load(libraryPath);
        }

        IntPtr INativeLibraryLoader.GetExport(IntPtr handle, string name) {
          Trace.TraceInformation($"Kernel32.GetExport({handle},\"{name}\")");
          return GetExport(handle, name);
        }

        public void Init() {
        }

      }

    }
#endif

  }

}
