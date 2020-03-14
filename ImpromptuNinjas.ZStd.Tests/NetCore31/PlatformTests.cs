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
    public void TargetFrameworkCheck() {
      var tf = typeof(ZStdException).Assembly.GetCustomAttribute<TargetFrameworkAttribute>();

      tf?.FrameworkName.Should().Be(".NETCoreApp,Version=v3.1");

    }

  }

}
