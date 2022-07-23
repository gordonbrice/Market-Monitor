using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeServices
{
    public interface IConsensusClientService : IClientService
    {
        Task<BlockHeaders> GetBeaconHeaders();
    }
}
