using CoinMarketCap;
using CypherUtil;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
        }
    }

    public class ApiKeyEntity : TableEntity
    {
        public string ApiKey { get; set; }
    }

}
