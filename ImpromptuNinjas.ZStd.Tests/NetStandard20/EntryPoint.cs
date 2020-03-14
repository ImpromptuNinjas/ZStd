using System;
using System.Diagnostics;
using NUnitLite;

internal static class EntryPoint {

  private static int Main(string[] args) {
    AppDomain.CurrentDomain.AssemblyResolve += (sender, eventArgs) => {
      Console.WriteLine($"{eventArgs.RequestingAssembly?.FullName} is requesting the resoltion of Assembly: {eventArgs.Name}");
      return null;
    };
    AppDomain.CurrentDomain.TypeResolve += (sender, eventArgs) => {
      Console.WriteLine($"{eventArgs.RequestingAssembly?.FullName} is requesting the resoltion of Type: {eventArgs.Name}");
      return null;
    };
    AppDomain.CurrentDomain.AssemblyLoad += (sender, eventArgs) => {
      Console.WriteLine($"Loaded: {eventArgs.LoadedAssembly?.FullName}");
    };
    AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) => {
      Console.WriteLine($"Exception: {eventArgs.Exception}");
    };
    AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) => {
      Console.WriteLine($"Unhandled Exception: {eventArgs.ExceptionObject}");
    };
    AppDomain.CurrentDomain.DomainUnload += (sender, eventArgs) => {
      Console.WriteLine($"Unloading");
    };
    Trace.Listeners.Add(new ConsoleTraceListener());
    Trace.AutoFlush = true;
    Debug.AutoFlush = true;

    Console.WriteLine("Beginning NUnit...");

    var result = new AutoRun().Execute(args);

    Console.WriteLine($"Finished: {result}");

    return result;
  }

}
