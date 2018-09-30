using CoinMarketCap;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using MvvmHelpers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MarketMonitor.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            Title = "EOS AssetMonitor";
            //apiKey = GetApiKey().Result;
            //GetGlobalMetrics();
            RefreshCommand = new Command(async () => await Refresh(), () => IsNotBusy);
        }

        public ICommand RefreshCommand { get; private set; }

        string name;
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

        string apiKey;

        private async Task Refresh()
        {
            IsBusy = true;

            if(string.IsNullOrEmpty(apiKey))
            {
                apiKey = await GetApiKey();
            }
            else
            {
                var result = await GlobalMetricsRequest.GetGlobalMetrics(apiKey);

                if(result == "Unauthorized")
                {
                    RawData = result;
                }
                else
                {
                    SetPropertiesFromGlobalMetrics(GlobalMetrics.FromJson(result));
                }
            }
            IsBusy = false;
        }

        private async Task<string> GetApiKey()
        {
            var account = new CloudStorageAccount(
                        new StorageCredentials("apikeystore", "faJKla6D7T/1vrzlOb7X1gtrlTrUEePqwudvN1xY9amjQ1X42cpDPx8Jo3WO904ppuqJeWw+FewSK8gFKp56TA=="), true);
            var tableClient = account.CreateCloudTableClient();
            var table = tableClient.GetTableReference("ApiKeys");

            //TableQuery<ApiKeyEntity> query = new TableQuery<ApiKeyEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Dev"));
            TableQuery<ApiKeyEntity> query = new TableQuery<ApiKeyEntity>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Dev"), TableOperators.And
                , TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "d666bdc6d2d2643ea61a6d5df444f6ef84e084b269565a2b0b5bf805fe36702b")));

            var result = await table.ExecuteQuerySegmentedAsync<ApiKeyEntity>(query, new TableContinuationToken());

            if(result.Results.Count > 0)
            {
                return result.Results[0].ApiKey;
            }

            return null;
        }

        private void SetPropertiesFromGlobalMetrics(GlobalMetrics globalMetrics)
        {
            TotalMarketCap = string.Format("{0:c}", globalMetrics.Data.Quote.Usd.TotalMarketCap);
            TotalVolume24H = string.Format("{0:n}", globalMetrics.Data.Quote.Usd.TotalVolume24H);
            TimeStamp = globalMetrics.Status.Timestamp.ToString();
            BtcDominance = string.Format("{0:n}%", globalMetrics.Data.BtcDominance);
            EthDominance = string.Format("{0:n}%", globalMetrics.Data.EthDominance);
        }

        private void GetGlobalMetrics()
        {
            try
            {
                if(!string.IsNullOrEmpty(apiKey))
                {
                    IsBusy = true;

                    var task = GlobalMetricsRequest.GetGlobalMetrics(apiKey);

                    task.Wait();
                    IsBusy = false;

                    if (task.IsCompleted)
                    {
                        //RawData = task.Result;
                        SetPropertiesFromGlobalMetrics(GlobalMetrics.FromJson(task.Result));
                    }
                }
            }
            catch (Exception e)
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
