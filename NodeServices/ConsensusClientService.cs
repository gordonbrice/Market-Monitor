using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NodeServices
{
    public class ClientVersion
    {
        public string Version { get; set; }
    }
    public class CLClientVersion
    {
        public ClientVersion data { get; set; }
    }
    public class ConsensusClientService : IConsensusClientService
    {
        string name;
        HttpClient httpClient = null;
        protected string uri = null;

        public string Name => this.name;

        public async Task<string> GetClientVersion()
        {
            var req = "/eth/v1/node/version";
            var response = await httpClient.GetAsync($"{this.uri}{req}");

            if (response.IsSuccessStatusCode)
            {
                var responseStr = await response.Content.ReadAsStringAsync();
                var clientVersion = JsonConvert.DeserializeObject<CLClientVersion>(responseStr);

                return clientVersion.data.Version;
            }

            return string.Empty;
        }

        public ConsensusClientService(string name, string uri, HttpClient httpClient)
        {
            this.name = name;
            this.uri = uri;
            this.httpClient = httpClient;
        }
    }
}
