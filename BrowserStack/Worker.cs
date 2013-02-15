using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrowserStack
{
  public sealed class Worker
  {
    public string Id { get; private set; }

    private readonly string authToken;
    private const string Url = "http://api.browserstack.com/3";

    internal Worker(string id, string authToken)
    {
      Id = id;
      this.authToken = authToken;
    }

    public TimeSpan Terminate()
    {
      using (var client = new WebClient())
      {
        client.Headers.Add("authorization", authToken);

        var data = client.UploadString(Url + "/worker/" + Id, "DELETE", string.Empty);
        return TimeSpan.FromSeconds(((JObject)JsonConvert.DeserializeObject(data))["time"].Value<double>());
      }
    }

    public WorkerStatus Status()
    {
      using (var client = new WebClient())
      {
        client.Headers.Add("authorization", authToken);

        var data = client.DownloadString(Url + "/worker/" + Id);
        var status = (JObject)JsonConvert.DeserializeObject(data);

        return new WorkerStatus()
        {
          Id = Id,
          Browser = new Browser()
          {
            OsName = status["os"].Value<string>(),
            OsVersion = status["os_version"].Value<string>(),
            BrowserName = status["browser"].Value<string>(),
            BrowserVersion = status["browser_version"].Value<string>()
          },
          Status = status["status"].Value<string>()
        };
      }
    }
  }
}
