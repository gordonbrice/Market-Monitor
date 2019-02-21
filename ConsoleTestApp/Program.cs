using BlockChain;
using System;
using System.Collections.Generic;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var ethAddrs = new Dictionary<string, string>();

            ethAddrs.Add();

            IBlockchain ethBC = new EtheriumBlockchain();
            var balTask = ethBC.GetBalance();

            balTask.Wait();

            Console.WriteLine($"Balance = ${balTask.Result}");
            Console.ReadLine();
        }
    }

}
