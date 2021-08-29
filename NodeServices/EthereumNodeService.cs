using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace NodeServices
{
    public class ClientInfo
    {
        public int? id { get; set; }
        public string jsonrpc { get; set; }
        public string result { get; set; }
    }

    public class EthereumNodeService : INodeService
    {
        Web3 web3 = null;
        string name;
        HttpClient httpClient = null;
        protected string uri = null;

        public event EventHandler<EthNodeServiceErrorEventArgs> Error;

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        //public string Uri { get; }

        public EthereumNodeService(string name, string uri, HttpClient httpClient)
        {
            this.name = name;
            this.uri = uri;
            this.httpClient = httpClient;
            this.web3 = new Web3(uri);
        }

        public virtual async Task<string> GetProtocolVersion()
        {
            return await web3.Eth.ProtocolVersion.SendRequestAsync();
        }

        public virtual async Task<string> GetClientVersion()
        {
            var arr = new JArray();
            var paramData = new object[] { "id", 67 };

            arr.Add(paramData);
            var payload = "{\"jsonrpc\":\"2.0\",\"method\":\"web3_clientVersion\",\"params\":[],\"id\":67}";
            var stringContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(this.uri, stringContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if(!string.IsNullOrEmpty(json))
                {
                    if (json.Contains("Parse Error"))
                    {
                        return "Parse Error";
                    }
                    else if (json.Contains("ID is not set"))
                    {
                        return "ID is not set";
                    }
                    else if (json.Contains("Relay attempts exhausted"))
                    {
                        return "Relay attempts exhausted";
                    }
                    else
                    {
                        var res = JsonConvert.DeserializeObject<ClientInfo>(json);

                        return res.result;
                    }
                }

                return "None";
            }

            return "Error";
        }

        public virtual async Task<HexBigInteger> GetChainId()
        {
            return await web3.Eth.ChainId.SendRequestAsync();
        }

        public virtual async Task<HexBigInteger> GetHighestBlock()
        {
            return await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        }

        public virtual async Task<SyncingOutput> GetSyncing()
        {
            try
            {
                return await web3.Eth.Syncing.SendRequestAsync();
            }
            catch(Exception e)
            {
                OnError(this.name, e.Message);
            }

            return null;
        }

        public async Task<HexBigInteger> GetBalance(string address)
        {
            return await web3.Eth.GetBalance.SendRequestAsync(address);
        }

        public Contract GetContract(string contractAbi, string contractAddress)
        {
            return web3.Eth.GetContract(contractAbi, contractAddress);
        }

        private void OnError(string name, string message)
        {
            if (Error != null)
            {
                Error(this, new EthNodeServiceErrorEventArgs
                {
                    Name = name,
                    Message = message
                });
            }
        }

    }
}
