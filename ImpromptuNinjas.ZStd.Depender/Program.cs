using static System.Console;

namespace ImpromptuNinjas.ZStd {

  internal static class Program {

    private static void Main(string[] args)
      => WriteLine(Native.ZStd.GetVersionString());

  }

}
