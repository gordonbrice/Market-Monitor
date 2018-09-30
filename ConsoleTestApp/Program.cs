using CoinMarketCap;
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
            //string response;
            //try
            //{
            //    var t = GlobalMetricsRequest.GetGlobalMetrics();

            //    t.Wait();
            //    response = t.Result;
            //}
            //catch (Exception e)
            //{
            //    response = e.Message;
            //}

            //Console.WriteLine(response);
            if(args.Length > 0)
            {
                var account = new CloudStorageAccount(
                            new StorageCredentials("apikeystore", "faJKla6D7T/1vrzlOb7X1gtrlTrUEePqwudvN1xY9amjQ1X42cpDPx8Jo3WO904ppuqJeWw+FewSK8gFKp56TA=="), true);
                var tableClient = account.CreateCloudTableClient();
                var table = tableClient.GetTableReference("ApiKeys");

                for (int y = 0; y < args.Length; y++)
                {
                    var bytes = Encoding.UTF8.GetBytes(args[y]);
                    var hashstring = new SHA256Managed();
                    var hash = hashstring.ComputeHash(bytes);
                    var hashString = new StringBuilder();

                    foreach (byte x in hash)
                    {
                        hashString.AppendFormat("{0:x2}", x);
                    }

                    //var readOperation = TableOperation.Retrieve("Dev", hashString.ToString());
                    // Execute the insert operation. 
                    //var result = table.Execute(readOperation);
                    TableQuery<ApiKeyEntity> query = new TableQuery<ApiKeyEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Dev"));

                    // Print the fields for each customer.
                    foreach (ApiKeyEntity entity in table.ExecuteQuery(query))
                    {
                        Console.WriteLine("Key:{0}, ApiKey:{1}", entity.RowKey, entity.ApiKey);
                    }



                    //Console.WriteLine(string.Format("Key:{0}, ApiKey:{1}", hashString.ToString(), result.Result.Property.ToString()));
                }
            }


            Console.ReadKey();
        }
    }

    public class ApiKeyEntity : TableEntity
    {
        public string ApiKey { get; set; }
    }

}
