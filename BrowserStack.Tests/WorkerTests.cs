using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrowserStack.Tests
{
  [TestClass]
  public class WorkerTests
  {
    [TestMethod]
    public void CanCreateWorker()
    {
      //Arrange
      var stack = new BrowserStack(ConfigurationManager.AppSettings["username"],
                                   ConfigurationManager.AppSettings["password"]);
      var browsers = stack.Browsers();

      //Act
      var worker = stack.CreateWorker(browsers.First(), "http://www.google.com");

      //Assert
      Assert.IsNotNull(worker);
      Assert.IsNotNull(worker.Id);

      //Kill the worker
      worker.Terminate();
    }

    [TestMethod]
    public void NewlyCreatedWorkerShouldHaveAValidStatus()
    {
      //Arrange
      //Arrange
      var stack = new BrowserStack(ConfigurationManager.AppSettings["username"],
                                   ConfigurationManager.AppSettings["password"]);
      var browsers = stack.Browsers();

      var worker = stack.CreateWorker(browsers.First(), "http://www.google.com");

      //Act
      var status = worker.Status();

      //Assert
      Assert.IsNotNull(status);
      Assert.IsNotNull(status.Status);

      //Kill the worker
      worker.Terminate();
    }

    [TestMethod]
    public void NewlyCreatedWorkerShouldTheRequestedBrowser()
    {
      //Arrange
      var stack = new BrowserStack(ConfigurationManager.AppSettings["username"],
                                   ConfigurationManager.AppSettings["password"]);
      var browsers = stack.Browsers();
      var browser = browsers.First();
      var worker = stack.CreateWorker(browser, "http://www.google.com");

      //Act
      var status = worker.Status();

      //Assert
      Assert.IsNotNull(status.Browser);
      Assert.AreEqual(browser.BrowserName, status.Browser.BrowserName);
      Assert.AreEqual(browser.BrowserVersion, status.Browser.BrowserVersion);

      //Kill the worker
      worker.Terminate();
    }

    [TestMethod]
    public void CanResolveWorkerStatusById()
    {
      //Arrange
      var stack = new BrowserStack(ConfigurationManager.AppSettings["username"],
                                   ConfigurationManager.AppSettings["password"]);
      var browsers = stack.Browsers();
      var browser = browsers.First();
      var worker = stack.CreateWorker(browser, "http://www.google.com");

      //Act
      var workers = stack.Workers();

      //Assert
      Assert.IsTrue(workers.Any(status => status.Id == worker.Id));
    }
  }
}
