using CoinMarketCap;
using CypherUtil;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using MvvmHelpers;
using Nomics;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MarketMonitor.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            IsNotLoggedIn = true;
            Title = "EOS AssetMonitor";
            RefreshCommand = new Command(async () => await Refresh(), () => IsNotBusy);
            LoginCommand = new Command(async () => await Login());
        }

        public ICommand RefreshCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }

        string name;
        string apiKeyCMC;
        string apiKeyNomics;

        public string RawData
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
            }
        }

        string totalMarketCap;
        public string TotalMarketCap
        {
            get { return totalMarketCap; }
            set
            {
                SetProperty(ref totalMarketCap, value);
            }
        }

        string totalVolume24H;
        public string TotalVolume24H
        {
            get { return totalVolume24H; }
            set
            {
                SetProperty(ref totalVolume24H, value);
            }
        }

        string timeStamp;
        public string TimeStamp
        {
            get { return timeStamp; }
            set
            {
                SetProperty(ref timeStamp, value);
            }
        }

        string btcDominance;
        public string BtcDominance
        {
            get { return btcDominance; }
            set
            {
                SetProperty(ref btcDominance, value);
            }
        }

        string ethDominance;
        public string EthDominance
        {
            get { return ethDominance; }
            set
            {
                SetProperty(ref ethDominance, value);
            }
        }

        bool isNotLoggedIn;
        public bool IsNotLoggedIn
        {
            get
            {
                return isNotLoggedIn;
            }

            set
            {
                SetProperty(ref isNotLoggedIn, value);
            }
        }

        string password;
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                SetProperty(ref password, value);
            }
        }
        private async Task Login()
        {
            await Refresh();
            IsNotLoggedIn = false;
        }

        private async Task Refresh()
        {
            IsBusy = true;

            if(string.IsNullOrEmpty(apiKeyCMC) || string.IsNullOrEmpty(apiKeyNomics))
            {
                if(string.IsNullOrEmpty(Password))
                {
                    RawData = "Password not set.";
                }
                else
                {
                    await GetApiKeys(Password);
                }
            }

            if(!string.IsNullOrEmpty(apiKeyCMC))
            {
                var resultCMC = await GlobalMetricsRequest.GetGlobalMetrics(apiKeyCMC);

                if(resultCMC == "Unauthorized")
                {
                    RawData = resultCMC;
                }
                else
                {
                    SetPropertiesFromGlobalMetrics(GlobalMetrics.FromJson(resultCMC));
                }
            }

            if(!string.IsNullOrEmpty(apiKeyNomics))
            {
                RawData = await MarketCapHistory.GetMarketCapHistory(apiKeyNomics);
            }

            IsBusy = false;
        }

        private async Task GetApiKeys(string password)
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

            if(result.Results.Count > 0)
            {
                apiKeyCMC = result.Results[0].ApiKey;
            }

            query = new TableQuery<ApiKeyEntity>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Dev"), TableOperators.And
                , TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "7f0889c0a7ec97519dff1cf6e1ed52a99422928f5412d53e01e5d546398da472")));

            result = await table.ExecuteQuerySegmentedAsync<ApiKeyEntity>(query, new TableContinuationToken());

            if (result.Results.Count > 0)
            {
                apiKeyNomics = result.Results[0].ApiKey;
            }
        }

        private void SetPropertiesFromGlobalMetrics(GlobalMetrics globalMetrics)
        {
            TotalMarketCap = string.Format("{0:c}", globalMetrics.Data.Quote.Usd.TotalMarketCap);
            TotalVolume24H = string.Format("{0:n}", globalMetrics.Data.Quote.Usd.TotalVolume24H);
            TimeStamp = globalMetrics.Status.Timestamp.ToString();
            BtcDominance = string.Format("{0:n}%", globalMetrics.Data.BtcDominance);
            EthDominance = string.Format("{0:n}%", globalMetrics.Data.EthDominance);
        }

        private async Task<string> GetGlobalMetrics()
        {
            try
            {
                if(!string.IsNullOrEmpty(apiKeyCMC))
                {
                    return await GlobalMetricsRequest.GetGlobalMetrics(apiKeyCMC);
                }
            }
            catch (Exception e)
            {
                RawData = e.Message;
            }

            return null;
        }

        private async Task GetMarketCapHistory()
        {
            try
            {
                if(!string.IsNullOrEmpty(apiKeyNomics))
                {
                    RawData = await MarketCapHistory.GetMarketCapHistory(apiKeyNomics);
                }
            }
            catch(Exception e)
            {
                RawData = e.Message;
            }
        }
    }

    public class ApiKeyEntity : TableEntity
    {
        public string ApiKey { get; set; }
    }
}
