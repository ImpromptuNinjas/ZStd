using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using FluentAssertions;
#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;

#endif

namespace ImpromptuNinjas.ZStd.Tests {

#if MSTEST
  [TestClass]
#else
  [TestFixture]
#endif
  public partial class PlatformTests {

#if MSTEST
    [TestMethod,UseParameterValues]
#else
    [Test]
#endif
    public void SanityCheck() {
      var zstdAsm = typeof(ZStdException).Assembly;

      var tf = zstdAsm.GetCustomAttribute<TargetFrameworkAttribute>();

      var dir = Path.GetDirectoryName(new Uri(zstdAsm.CodeBase!).LocalPath)!;

      string commit = null;
      using (var proc = Process.Start(new ProcessStartInfo("git", "rev-parse HEAD") {
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        WorkingDirectory = dir
      })) {
        proc?.Start();
        commit = proc?.StandardOutput.ReadToEnd()?.Trim();
        proc?.StandardError.ReadToEnd().Should().BeNullOrWhiteSpace();
      }

      commit.Should().NotBeNullOrEmpty();

      Console.WriteLine($"Current commit according to git: {commit}");

      var version = zstdAsm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

      version.Should().NotBeNullOrEmpty();

#if NETFRAMEWORK
      (version?.Split(new[] {'+'}, 2)[1]).Should().Be(commit);
#else
      (version?.Split('+', 2)[1]).Should().Be(commit);
#endif

      Console.WriteLine($"Assembly Informational Version: {version}");

      Console.WriteLine($"Assembly Framework: {tf?.FrameworkName}");

      Console.WriteLine($"Runtime Framework Description: {RuntimeInformation.FrameworkDescription}");

      Console.WriteLine($"Runtime OS Version: {Environment.OSVersion}");

      Console.WriteLine($"Runtime Framework OS Description: {RuntimeInformation.OSDescription}");

      Console.WriteLine($"Runtime Framework OS Architecture: {RuntimeInformation.OSArchitecture}");

      Console.WriteLine($"Common Language Runtime Version: {Environment.Version}");

      Console.WriteLine($"Runtime Framework Process Architecture: {RuntimeInformation.ProcessArchitecture}");

      Console.WriteLine($"Runtime Framework Version: {RuntimeEnvironment.GetSystemVersion()}");

      Console.WriteLine($"Runtime Framework Directory: {RuntimeEnvironment.GetRuntimeDirectory()}");

      Console.WriteLine($"Processor Count: {Environment.ProcessorCount}");

      Console.WriteLine("Assembly Metadata:");
      foreach (var meta in zstdAsm.GetCustomAttributes<AssemblyMetadataAttribute>())
        Console.WriteLine($"  {meta.Key}: {meta.Value}");
    }

  }

}
