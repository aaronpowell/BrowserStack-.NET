using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace BrowserStack
{
    public sealed class Worker
    {
        public string Id { get; private set; }

        private readonly string authToken;
        private const string Url = "http://api.browserstack.com/1";

        internal Worker(string id, string authToken)
        {
            Id = id;
            this.authToken = authToken;
        }

        public void Terminate()
        {
            var request = WebRequest.Create(Url + "/worker/" + Id);
            request.Headers.Add("authorization", authToken);

            request.Method = "DELETE";

            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ArgumentException("The work id provided is not owned by the authenticed user");
        }

        public WorkerStatus Status()
        {
            var request = WebRequest.Create(Url + "/worker/" + Id);
            request.Headers.Add("authorization", authToken);

            var response = request.GetResponse();

            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                var data = stream.ReadToEnd();

                var status = JsonConvert.DeserializeObject<WorkerStatus>(data);

                status.Id = Id;

                return status;
            }
        }
    }
}
