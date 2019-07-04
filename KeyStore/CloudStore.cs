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
            //var encrypted = Symmetric.Encrypt<AesManaged>(decrypted, password1, password2);
            var encrypted = "Ln9CeqVMYIxTb7Aj15bxf6elCWuzGT0ofThQr/lbWTJYEVjWyMSnzQCuUNWqfu1etRAjn2tbcjkDHi7Y4IA26Fw1+MGqqolTfkQhZpifNqZLxG3pxN9HxiBTdOB1t1cAeQmme2zWop/BYoROvgdYNSWloZTGrO4XIixRe/9ufOGUBXxovJKL97c/nDmTFSMA50g1KUOUUKZeBcwA0iFM38vbFxj1ryqFL8qHTznrY00xgkprXMTT/yNldhnpbmlz";
            var decrypted = Symmetric.Decrypt<AesManaged>(encrypted, password1, password2);
            var account = new CloudStorageAccount(new StorageCredentials("apikeystore", decrypted), true);
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
