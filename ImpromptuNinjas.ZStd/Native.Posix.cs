using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace ImpromptuNinjas.ZStd {

  public static partial class Native {


    private static bool IsMusl() {
      using (var proc = Process.GetCurrentProcess()) {
        foreach (ProcessModule mod in proc.Modules) {
          var fileName = mod.FileName;

          if (!fileName.Contains("libc"))
            continue;

          if (fileName.Contains("musl"))
            return true;

          break;
        }
      }

      return false;
    }


    private static string GetProcArchString() {
      var cpu = RuntimeInformation.ProcessArchitecture;
      switch (cpu) {
        case Architecture.X86:
          return "x86";
        case Architecture.X64:
          return "x64";
        case Architecture.Arm:
          return "arm";
        case Architecture.Arm64:
          return "arm64";
        default: throw new PlatformNotSupportedException(cpu.ToString());
      }
    }

  }

}
