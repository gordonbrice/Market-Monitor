using CypherUtil;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace KeyStore
{
    public class ApiKeyEntity : TableEntity
    {
        public string ApiKey { get; set; }
    }

    public class CloudStore
    {
        public string CoinMarketCapApiKey { get; private set; }
        public string NomicsApiKey { get; private set; }
        public string InfuraMainnetKey { get; private set; }
        public async Task GetApiKeys(string password1, string password2)
        {
            var encrypted = "WYxUEO7vhst3aQfDjVGOn72Q+Eo+Or9Z10i9pf3Vc+4Jqs25GQvhD4L7Fplghtm5xtt/RUGvl2VYO/MaTbarHjj5f544sKjopWNu6PwxS67CiUUFxVlpQZx8ehvGrSCjDfUjJ2CPtFomDc9cy/8k33h1QOSX8Qp88m+z2AyxeUjA6YBIHNvFTbYukc3OOJlfYQMP6cSwQ43J9DK0CwwgsvJYBQkHyjoqHzI5d7OII20q462vLdBxEfGMc2zkCdLX";
            var decrypted = Symmetric.Decrypt<AesManaged>(encrypted, password1, password2);

            var account = new CloudStorageAccount(
                        new StorageCredentials("apikeystore", decrypted), true);
            var tableClient = account.CreateCloudTableClient();
            var table = tableClient.GetTableReference("ApiKeys");

            TableQuery<ApiKeyEntity> query = new TableQuery<ApiKeyEntity>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Dev"), TableOperators.And
                , TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "d666bdc6d2d2643ea61a6d5df444f6ef84e084b269565a2b0b5bf805fe36702b")));

            var result = await table.ExecuteQuerySegmentedAsync<ApiKeyEntity>(query, new TableContinuationToken());

            if (result.Results.Count > 0)
            {
                CoinMarketCapApiKey = result.Results[0].ApiKey;
            }

            query = new TableQuery<ApiKeyEntity>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Dev"), TableOperators.And
                , TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "7f0889c0a7ec97519dff1cf6e1ed52a99422928f5412d53e01e5d546398da472")));

            result = await table.ExecuteQuerySegmentedAsync<ApiKeyEntity>(query, new TableContinuationToken());

            if (result.Results.Count > 0)
            {
                NomicsApiKey = result.Results[0].ApiKey;
            }

            query = new TableQuery<ApiKeyEntity>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Dev"), TableOperators.And
                , TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "103ed9f43eb647a0986e751faa8c5745")));

            result = await table.ExecuteQuerySegmentedAsync<ApiKeyEntity>(query, new TableContinuationToken());

            if (result.Results.Count > 0)
            {
                InfuraMainnetKey = result.Results[0].ApiKey;
            }
        }
    }
}
