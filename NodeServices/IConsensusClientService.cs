using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeServices
{
    public interface IConsensusClientService : IClientService
    {
        Task<BlockHeaders> GetBeaconHeaders();
        Task<SingleBlockHeader> GetBlockHeader(string blockId);
        Task SubscribeToEvent(string eventName);
    }
}
