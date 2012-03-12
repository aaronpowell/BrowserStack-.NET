using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrowserStack
{
    public class BrowserStack
    {
        private readonly string authToken;
        private const string Url = "http://api.browserstack.com/1";

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
            var request = WebRequest.Create(Url + "/browsers");
            request.Headers.Add("authorization", authToken);

            var response = request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var data = reader.ReadToEnd();

                var browsers = (JArray)JsonConvert.DeserializeObject(data);

                return browsers.Select(browser => new Browser
                                                      {
                                                          Name = ((JObject) browser)["browser"].Value<string>(),
                                                          Version = ((JObject) browser)["version"].Value<string>()
                                                      });
            }
        }

        public Worker CreateWorker(Browser browser, string url, int timeout = 30)
        {
            var request = WebRequest.Create(Url + "/worker");
            request.Headers.Add("authorization", authToken);

            request.Method = "POST";

            var postString = "browser=" + browser.Name + "&version=" + browser.Version + "&url=" + url + "&timeout=" +
                           timeout;
            var postBytes = Encoding.ASCII.GetBytes(postString);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;

            using (var postStream = request.GetRequestStream())
                postStream.Write(postBytes, 0, postBytes.Length);

            var response = request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var data = reader.ReadToEnd();

                var id = JsonConvert.DeserializeAnonymousType(data, new { id = "asd" }).id;

                return new Worker(id, authToken);
            }
        }

        public IEnumerable<WorkerStatus> Workers()
        {
            var request = WebRequest.Create(Url + "/workers");
            request.Headers.Add("authorization", authToken);

            var response = request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var data = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<IEnumerable<WorkerStatus>>(data);
            }
        }
    }
}
