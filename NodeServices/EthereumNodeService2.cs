using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NodeServices
{
    public class EthereumNodeService2 : EthereumNodeService
    {
        HttpClient httpClient = null;

        public EthereumNodeService2(string name, string uri) : base(name, uri)
        {
            this.httpClient = new HttpClient();

        }
        public override async Task<string> GetProtocolVersion()
        {
            return await httpClient.GetStringAsync(this.uri);
        }
        public new async Task<string> GetSyncing()
        {
            return await httpClient.GetStringAsync(this.uri);
        }
        public new async Task<string> GetHighestBlock()
        {
            return await httpClient.GetStringAsync(this.uri);
        }
    }
}
