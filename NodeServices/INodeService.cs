using Nethereum.Hex.HexTypes;
using System;
using System.Collections.Generic;
using Nethereum.Contracts;
using System.Text;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;

namespace NodeServices
{
    public interface INodeService
    {
        string Name { get; }
        Task<string> GetProtocolVersion();
        Task<HexBigInteger> GetChainId();
        Task<HexBigInteger> GetHighestBlock();
        Task<SyncingOutput> GetSyncing();
        Task<HexBigInteger> GetBalance(string address);
        Contract GetContract(string contractAbi, string contractAddress);
    }
}
