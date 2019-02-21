using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    public class EtheriumBlockchain : IBlockchain
    {
        public async Task<decimal> GetBalance(string address)
        {
            var web3 = new Web3("https://mainnet.infura.io");
            var balance = await web3.Eth.GetBalance.SendRequestAsync(address);

            return Web3.Convert.FromWei(balance);
        }
    }
}
