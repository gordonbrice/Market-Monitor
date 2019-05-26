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

            var quotesTask = CryptoCurrencysRequest.GetQuotes(keyStore.CoinMarketCapApiKey, "BTC,ETH,EOS,USDT,BNB,ADA,MKR,USDC,TUSD,REP,PAX,DAI");

            quotesTask.Wait();

            var quote = quotesTask.Result;
            var cryptoQuote = CryptoCurrencies.FromJson(quote);
            decimal ethUsd = 0.00M;

            if (cryptoQuote.Data.ContainsKey("ETH"))
            {
                Datum data = null;

                if (cryptoQuote.Data.TryGetValue("ETH", out data))
                {
                    ethUsd = (decimal)data.Quote.Usd.Price;
                }
            }

            decimal btcUsd = 0.00M;

            if (cryptoQuote.Data.ContainsKey("BTC"))
            {
                Datum data = null;

                if (cryptoQuote.Data.TryGetValue("BTC", out data))
                {
                    btcUsd = (decimal)data.Quote.Usd.Price;
                }
            }

            StringBuilder quotes = new StringBuilder();

            foreach(var data in cryptoQuote.Data)
            {
                quotes.Append($"{data.Key} = ${data.Value.Quote.Usd.Price}\n");
            }

            Console.WriteLine(quotes);

            #region BTC

            //var btcAddrStr = File.ReadAllText(@"c:\Apps\Test\btc.txt").Trim();
            //var btcAddresses = btcAddrStr.Split('|');
            //IBlockchain btcBC = new BitcoinBlockchain();

            //foreach (var btcAddress in btcAddresses)
            //{
            //    if (!string.IsNullOrEmpty(btcAddress.Trim()))
            //    {
            //        var addr = btcAddress.Split(',');
            //        var balTask = btcBC.GetBalance(addr[0]);

            //        balTask.Wait();

            //        Console.WriteLine($"{addr[1]} Balance = BTC:{balTask.Result}, ${balTask.Result * btcUsd}\n");
            //    }
            //}

            #endregion

            #region ETH
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

                    Console.WriteLine($"{addr[1]} Balance = ETH:{balTask.Result}, ${balTask.Result * ethUsd}\n");
                }
            }
            #endregion
            Console.ReadLine();
        }
    }

}
