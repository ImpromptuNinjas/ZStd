using System;
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

  public partial class PlatformTests {

#if MSTEST
    [TestMethod,UseParameterValues]
#else
    [Test]
#endif
    public unsafe void TargetFrameworkCheck() {
      sizeof(void*).Should().Be(4);

      var tf = typeof(ZStdException).Assembly.GetCustomAttribute<TargetFrameworkAttribute>();

      tf?.FrameworkName.Should().Be(".NETFramework,Version=v4.5");

      Console.WriteLine(tf?.FrameworkName);
    }

  }

}
