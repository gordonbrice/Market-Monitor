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

        public async Task GetApiKeys(string password)
        {
            //mIWrBwmCu+/ZvZfwS2//R5YKYokBuIo7BRixpa1dpzY9EY6nV/7FMbR8aVC7D5okOU38a2QPYxLxK/Qsf6DqplVbU2irUtYEcBuxJt9wymu8KqiQLQIkd+lHUTxJxzmEGKnmI9JcVscWF5mkj9XANx5263PP7xh47d4NbDOSwn8Jud85ZZtvlWroemb9U2JI2uf/5I2hzhX7Op/shEVFDQbnM9YkJ3coIGRJm+cD9x2h7cDjYnyR1dNr1fJwB6Dx
            string decrypted = Symmetric.Decrypt<AesManaged>("mIWrBwmCu+/ZvZfwS2//R5YKYokBuIo7BRixpa1dpzY9EY6nV/7FMbR8aVC7D5okOU38a2QPYxLxK/Qsf6DqplVbU2irUtYEcBuxJt9wymu8KqiQLQIkd+lHUTxJxzmEGKnmI9JcVscWF5mkj9XANx5263PP7xh47d4NbDOSwn8Jud85ZZtvlWroemb9U2JI2uf/5I2hzhX7Op/shEVFDQbnM9YkJ3coIGRJm+cD9x2h7cDjYnyR1dNr1fJwB6Dx"
                , password, "apikeystore");

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
        }
    }
}
