using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CoinMarketCap;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MarketMonitor
{
    public partial class App : Application
    {
        //private string displayText = "Getting Data...";

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            //MainPage = new ContentPage
            //{
            //    Content = new StackLayout
            //    {
            //        VerticalOptions = LayoutOptions.Center,
            //        Children = {
            //            new Label {
            //                HorizontalTextAlignment = TextAlignment.Center,
            //                Text = displayText
            //            }
            //        }
            //    }
            //};
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
