using CoinMarketCap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string response;
            try
            {
                var t = GlobalMetricsRequest.GetGlobalMetrics();

                t.Wait();
                response = t.Result;
            }
            catch (Exception e)
            {
                response = e.Message;
            }

            Console.WriteLine(response);
            Console.ReadKey();
        }
    }
}
