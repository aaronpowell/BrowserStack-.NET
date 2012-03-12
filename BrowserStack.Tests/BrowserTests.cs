using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace BrowserStack.Tests
{
    [TestClass]
    public class BrowserTests
    {
        [TestMethod]
        public void ThereShouldBeAtLeastOneBrowser()
        {
            //Arrange
            var stack = new BrowserStack(ConfigurationManager.AppSettings["username"],
                                         ConfigurationManager.AppSettings["password"]);

            //Act
            var browsers = stack.Browsers();

            //Assert
            Assert.IsTrue(browsers.Any());
        }
    }
}
