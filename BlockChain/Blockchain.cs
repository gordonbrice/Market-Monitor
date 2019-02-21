using System;
using System.Threading.Tasks;

namespace BlockChain
{
    public interface IBlockchain
    {
        Task<decimal> GetBalance(string address);
    }
}
