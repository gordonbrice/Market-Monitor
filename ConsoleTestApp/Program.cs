using BlockChain;
using CoinMarketCap;
using KeyStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter password:");

            var password = Console.ReadLine();
            var keyStore = new CloudStore();

            var fetchKeysTask = keyStore.GetApiKeys(password);

            fetchKeysTask.Wait();

            var quotesTask = CryptoCurrencysRequest.GetQuotes(keyStore.CoinMarketCapApiKey, "ETH");

            quotesTask.Wait();

            var quote = quotesTask.Result;
            var cryptoQuote = CryptoCurrencies.FromJson(quote);

            var addressStr = File.ReadAllText(@"c:\Apps\Test\eth.txt").Trim();
            var addresses = addressStr.Split('|');
            IBlockchain ethBC = new EtheriumBlockchain();

            foreach(var address in addresses)
            {
                if(!string.IsNullOrEmpty(address.Trim()))
                {
                    var addr = address.Split(',');
                    var balTask = ethBC.GetBalance(addr[0]);

                    balTask.Wait();

                    Console.WriteLine($"{addr[1]} Balance = ETH:{balTask.Result}, ${balTask.Result * (decimal)cryptoQuote.Data.Eth.Quote.Usd.Price}\n");
                }
            }

            Console.ReadLine();
        }
    }

}
