using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrowserStack.Tests
{
  [TestClass]
  public class ConstructorTests
  {
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EmptyUsernameThrowsError()
    {
      new BrowserStack(string.Empty, string.Empty);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void EmptyPasswordThrowsError()
    {
      new BrowserStack("Foo", string.Empty);
    }
  }
}
