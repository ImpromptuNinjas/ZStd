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
  public partial class BindingTests {

  }

}
