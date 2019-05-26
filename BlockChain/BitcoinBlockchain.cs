using NBitcoin;
using NBitcoin.RPC;
using QBitNinja.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    public class BitcoinBlockchain : IBlockchain
    {
        public async Task<decimal> GetBalance(string address)
        {
            //var client = new QBitNinjaClient(Network.Main);

            //var balance = await client
            //Console.WriteLine(string.Format("TransId:{0}", transactionResponse.TransactionId));
            //Console.WriteLine(string.Format("Conformations:{0}", transactionResponse.Block.Confirmations));


            var targetAddr = new BitcoinWitPubKeyAddress(address);
            RPCCredentialString creds = new RPCCredentialString();
            RPCClient client = new RPCClient(Network.Main);

            client.ImportAddress(targetAddr);

            var bal = client.GetBalance();

            Console.WriteLine(bal);

            return decimal.Parse(bal.ToString());
        }
    }
}
