using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Threading.Tasks;

namespace NodeServices
{
    public interface INodeService
    {
        string Name { get; }
        Task<string> GetProtocolVersion();
        Task<HexBigInteger> GetHighestBlock();
        Task<SyncingOutput> GetSyncing();
        Task<HexBigInteger> GetBalance(string address);
        Contract GetContract(string contractAbi, string contractAddress);
    }
    public class EthereumNodeService : INodeService
    {
        Web3 web3 = null;

        string name;
        public string Name { get; }

        public string Uri { get; }

        public EthereumNodeService(string name, string uri)
        {
            this.name = string.Format("{0}-{1}", name, uri);
            this.web3 = new Web3(uri);
        }

        public async Task<string> GetProtocolVersion()
        {
            return await web3.Eth.ProtocolVersion.SendRequestAsync();

        }

        public async Task<HexBigInteger> GetHighestBlock()
        {
            return await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        }

        public async Task<SyncingOutput> GetSyncing()
        {
            return await web3.Eth.Syncing.SendRequestAsync();
        }

        public async Task<HexBigInteger> GetBalance(string address)
        {
            return await web3.Eth.GetBalance.SendRequestAsync(address);
        }

        public Contract GetContract(string contractAbi, string contractAddress)
        {
            return web3.Eth.GetContract(contractAbi, contractAddress);
        }

    }
}
