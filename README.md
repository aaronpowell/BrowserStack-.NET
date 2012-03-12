# BrowserStack .NET

This is an API which is inspired by [node-browserstack](https://github.com/scottgonzalez/node-browserstack) but written in .NET for use by .NET developers.

It is a wrapper for the [Automated Browser Testing API](http://www.browserstack.com/automated-browser-testing-api) from [BrowserStack](http://www.browserstack.com/), allowing you to run up new VMs for testing your web applications from a .NET programming model.

This is developed against the v1.0 API from BrowserStack.

# Installation

You can install it from NuGet:

	Install-Package BrowserStack
	
# Usage

To use it first you need to have an account at [BrowserStack](http://www.browserstack.com/) (with credit!) and then you can use it like so:

	var stack = new BrowserStack("username", "password");
	
	var browsers = stack.Browsers();
	
	var worker = stack.CreateWorker(browsers.First(browser => browser.Name == "ie" && browser.Version == "7.0"), "http://my-awesome-site.com");
	
You can query a worker for its status (to find out if it's still active or not):

	var status = worker.Status();
	Console.WriteLine(status.Status);
	
You can also kill active workers:

	worker.Terminate();
	
Or get the status of all active workers:

	var workers = stack.Workers();
	Console.WriteLine(workers.Count());
	
# License

[MIT](https://github.com/aaronpowell/BrowserStack-.NET/blob/master/LICENSE.md)