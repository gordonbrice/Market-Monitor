using BlockChain;
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
            var addressStr = File.ReadAllText(@"c:\Apps\Test\db.txt").Trim();
            var addresses = addressStr.Split('|');
            IBlockchain ethBC = new EtheriumBlockchain();

            foreach(var address in addresses)
            {
                if(!string.IsNullOrEmpty(address.Trim()))
                {
                    var addr = address.Split(',');
                    var balTask = ethBC.GetBalance(addr[0]);

                    balTask.Wait();

                    Console.WriteLine($"{addr[1]} Balance = ETH:{balTask.Result}");
                }
            }

            Console.ReadLine();
        }
    }

}
