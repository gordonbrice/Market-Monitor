using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks;

namespace NodeServices
{
    public interface INodeService
    {
        string Name { get; }
        Task<string> GetProtocolVersion();
        Task<HexBigInteger> GetChainId();
        Task<HexBigInteger> GetGasPrice();
        Task<HexBigInteger> GetHighestBlock();
        Task<SyncingOutput> GetSyncing();
        Task<HexBigInteger> GetBalance(string address);
        Contract GetContract(string contractAbi, string contractAddress);
        Task<string> GetClientVersion();

    }
}
