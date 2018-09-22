using CoinMarketCap;
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
            GetGlobalMetrics();
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


        private async Task Refresh()
        {
            IsBusy = true;

            var result = await GlobalMetricsRequest.GetGlobalMetrics();

            SetPropertiesFromGlobalMetrics(GlobalMetrics.FromJson(result));
            IsBusy = false;
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
                IsBusy = true;

                var task = GlobalMetricsRequest.GetGlobalMetrics();

                task.Wait();
                IsBusy = false;

                if (task.IsCompleted)
                {
                    //RawData = task.Result;
                    SetPropertiesFromGlobalMetrics(GlobalMetrics.FromJson(task.Result));

                }
            }
            catch (Exception e)
            {
                RawData = e.Message;
            }
        }
    }
}
