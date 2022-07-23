using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public class BeaconBlockHeader
    {
        public string slot { get; set; }
        public string proposer_index { get; set; }
        public string parent_root { get; set; }
        public string state_root { get; set; }
        public string body_root { get; set; }

    }
    public class BlockHeader
    {
        public string root { get; set; }
        public bool canonical { get; set; }
        public SignedBeaconBlockHeader header { get; set; }
    }
    public class BlockHeaders
    {
        public bool execution_optimistic { get; set; }
        public BlockHeader[] data { get; set; }
    }
    public class SignedBeaconBlockHeader
    {
        public BeaconBlockHeader message { get; set; }
        public string signature { get; set; }
    }
    public class ConsensusClientService : IConsensusClientService
    {
        string name;
        HttpClient httpClient = null;
        protected string uri = null;
        protected string auth = null;

        public string Name => this.name;

        public ConsensusClientService(string name, string uri, HttpClient httpClient, string auth = null)
        {
            this.name = name;
            this.uri = uri;
            this.httpClient = httpClient;
            this.auth = auth;
        }

        public async Task<string> GetClientVersion()
        {
            var req = "/eth/v1/node/version";

            this.httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(auth)));

            var response = await httpClient.GetAsync($"{this.uri}{req}");

            this.httpClient.DefaultRequestHeaders.Clear();

            if (response.IsSuccessStatusCode)
            {
                var responseStr = await response.Content.ReadAsStringAsync();
                var clientVersion = JsonConvert.DeserializeObject<CLClientVersion>(responseStr);

                return clientVersion.data.Version;
            }

            return string.Empty;
        }
        public async Task<BlockHeaders> GetBeaconHeaders()
        {
            var req = "/eth/v1/beacon/headers";

            this.httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(auth)));

            var response = await httpClient.GetAsync($"{this.uri}{req}");

            this.httpClient.DefaultRequestHeaders.Clear();

            if (response.IsSuccessStatusCode)
            {
                var responseStr = await response.Content.ReadAsStringAsync();
                var blockHeaders = JsonConvert.DeserializeObject<BlockHeaders>(responseStr);

                return blockHeaders;
            }

            return null;
        }
    }
}
