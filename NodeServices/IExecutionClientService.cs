using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks;

namespace NodeServices
{
    public interface IExecutionClientService : IClientService
    {
        Task<string> GetProtocolVersion();
        Task<HexBigInteger> GetChainId();
        Task<HexBigInteger> GetGasPrice();
        Task<HexBigInteger> GetHighestBlock();
        Task<SyncingOutput> GetSyncing();
        Task<HexBigInteger> GetBalance(string address);
        Contract GetContract(string contractAbi, string contractAddress);

    }
}
