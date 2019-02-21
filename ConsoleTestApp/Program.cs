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

            ethAddrs.Add("0x771401d1a2a9BB5d5f6b2Fd5C870E0962e697358","Jaxx SALT Addr");

            IBlockchain ethBC = new EtheriumBlockchain();

            foreach(var key in ethAddrs.Keys)
            {
                var balTask = ethBC.GetBalance(key);

                balTask.Wait();

                Console.WriteLine($"Balance = ETH:{balTask.Result}");
            }
            Console.ReadLine();
        }
    }

}
