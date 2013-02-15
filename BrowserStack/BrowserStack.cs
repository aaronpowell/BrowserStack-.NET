using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrowserStack
{
  // https://github.com/browserstack/api

  public sealed class BrowserStack
  {
    private readonly string authToken;
    private const string Url = "http://api.browserstack.com/3";

    public BrowserStack(string username, string password)
    {
      if (string.IsNullOrEmpty(username))
        throw new ArgumentNullException("username");
      if (string.IsNullOrEmpty(password))
        throw new ArgumentNullException("password");

      authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
    }

    public IEnumerable<Browser> Browsers()
    {
      using (var client = new WebClient())
      {
        client.Headers.Add("authorization", authToken);

        var data = client.DownloadString(Url + "/browsers?flat=true");
        var browsers = (JArray)JsonConvert.DeserializeObject(data);

        return browsers.Select(browser => new Browser
                                          {
                                            Device = ((JObject)browser)["device"].Value<string>(),
                                            OsName = ((JObject)browser)["os"].Value<string>(),
                                            OsVersion = ((JObject)browser)["os_version"].Value<string>(),
                                            BrowserName = ((JObject)browser)["browser"].Value<string>(),
                                            BrowserVersion = ((JObject)browser)["browser_version"].Value<string>()
                                          });
      }
    }

    public ApiStatus GetApiStatus()
    {
      using (var client = new WebClient())
      {
        client.Headers.Add("authorization", authToken);

        var data = client.DownloadString(Url + "/status");
        var status = (JObject)JsonConvert.DeserializeObject(data);

        if (status["message"] != null)
        {
          return new ApiStatus()
          {
            Message = status["message"].Value<string>()
          };
        }

        return new ApiStatus()
        {
          UsedTime = TimeSpan.FromSeconds(status["used_time"].Value<double>()),
          AvailableTime = TimeSpan.FromSeconds(status["total_available_time"].Value<double>()),
          RunningWindowsSessions = status["running_windows_sessions"].Value<int>(),
          WindowsSessionsLimit = status["windows_sessions_limit"].Value<int>(),
          RunningMacSessions = status["running_mac_sessions"].Value<int>(),
          MacSessionsLimit = status["mac_sessions_limit"].Value<int>(),
        };
      }
    }

    public Worker CreateWorker(Browser browser, string url, int timeout = 30)
    {
      if (browser == null)
        throw new ArgumentNullException("browser");

      if (string.IsNullOrEmpty(url))
        throw new ArgumentNullException("url");

      using (var client = new WebClient())
      {
        client.Headers.Add("authorization", authToken);

        client.QueryString.Add("os", browser.OsName);
        client.QueryString.Add("os_version", browser.OsVersion);
        client.QueryString.Add("browser", browser.BrowserName);
        client.QueryString.Add("browser_version", browser.BrowserVersion);
        client.QueryString.Add("device", browser.Device);
        client.QueryString.Add("timeout", timeout.ToString());
        client.QueryString.Add("url", HttpUtility.UrlEncode(url));

        var data = client.UploadString(Url + "/worker", string.Empty);

        return new Worker(((JObject)JsonConvert.DeserializeObject(data))["id"].Value<string>(), authToken);
      }
    }

    public IEnumerable<WorkerStatus> Workers()
    {
      using (var client = new WebClient())
      {
        client.Headers.Add("authorization", authToken);

        var data = client.DownloadString(Url + "/workers");

        var status = (JArray)JsonConvert.DeserializeObject(data);

        return status.Select(s => new WorkerStatus()
                                  {
                                    Id = s["id"].Value<string>(),
                                    Browser = new Browser()
                                    {
                                      OsName = s["os"].Value<string>(),
                                      OsVersion = s["os_version"].Value<string>(),
                                      BrowserName = s["browser"].Value<string>(),
                                      BrowserVersion = s["browser_version"].Value<string>()
                                    },
                                    Status = s["status"].Value<string>()
                                  });
      }
    }
  }
}
