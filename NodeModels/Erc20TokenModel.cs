using Nethereum.Hex.HexTypes;
using NodeServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace NodeModels
{
    public class Erc20TokenModel : ViewModelBase
    {
        #region Mainnet Token Contract Addresses
        public static string DaiContractAddr = "0x89d24A6b4CcB1B6fAA2625fE562bDD9a23260359";
        public static string UsdcContractAddr = "0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48";
        public static string cDaiContractAddr = "0xf5dce57282a584d2746faf1593d3121fcac444dc";
        public static string TusdContractAddr = "0xcb9a11afdc6bdb92e4a6235959455f28758b34ba";
        public static string BnbContractAddr = "0xB8c77482e45F1F44dE1745F52C74426C631bDD52";
        public static string UsdtContractAddr = "0xdac17f958d2ee523a2206206994597c13d831ec7";
        #endregion

        INodeService ethereumService;

        string name;
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                OnPropertyChanged("Name");
            }
        }

        string fullName;
        public string FullName
        {
            get
            {
                return this.fullName;
            }
            set
            {
                this.fullName = value;
                OnPropertyChanged("FullName");
            }
        }
        string symbol;
        public string Symbol
        {
            get
            {
                return this.symbol;
            }
            set
            {
                this.symbol = value;
                OnPropertyChanged("Symbol");
            }
        }

        int decimals;
        public int Decimals
        {
            get
            {
                return this.decimals;
            }
            set
            {
                this.decimals = value;
                OnPropertyChanged("Decimals");
            }
        }

        BigInteger totalSupply;
        public BigInteger TotalSupply
        {
            get
            {
                return this.totalSupply;
            }
            set
            {
                this.totalSupply = value;
                OnPropertyChanged("TotalSupply");
            }
        }
        public Erc20TokenModel(string name, INodeService ethereumService)
        {
            this.ethereumService = ethereumService;
            Name = name;
        }

        public async void QueryProperties()
        {
            switch (this.name.ToLower())
            {
                case "dai":
                    var daiContractABI = File.ReadAllText(@"c:\Apps\Test\Contracts\ERC20Tokens\DAI_ABI.json").Trim();
                    var daiContract = this.ethereumService.GetContract(daiContractABI, Erc20TokenModel.DaiContractAddr);

                    FullName = await daiContract.GetFunction("name").CallAsync<string>();
                    Symbol = await daiContract.GetFunction("symbol").CallAsync<string>();
                    Decimals = await daiContract.GetFunction("decimals").CallAsync<int>();
                    TotalSupply = await daiContract.GetFunction("totalSupply").CallAsync<BigInteger>();
                    break;

                case "usdt":
                    var usdtContractABI = File.ReadAllText(@"c:\Apps\Test\Contracts\ERC20Tokens\USDT_ABI.json").Trim();
                    var usdtContract = this.ethereumService.GetContract(usdtContractABI, Erc20TokenModel.UsdtContractAddr);

                    FullName = await usdtContract.GetFunction("name").CallAsync<string>();
                    Symbol = await usdtContract.GetFunction("symbol").CallAsync<string>();
                    Decimals = await usdtContract.GetFunction("decimals").CallAsync<int>();
                    TotalSupply = await usdtContract.GetFunction("totalSupply").CallAsync<BigInteger>();
                    break;

                case "tusd":
                    var tusdContractABI = File.ReadAllText(@"c:\Apps\Test\Contracts\ERC20Tokens\TUSD_ABI.json").Trim();
                    var tusdContract = this.ethereumService.GetContract(tusdContractABI, Erc20TokenModel.TusdContractAddr);

                    FullName = await tusdContract.GetFunction("name").CallAsync<string>();
                    Symbol = await tusdContract.GetFunction("symbol").CallAsync<string>();
                    Decimals = await tusdContract.GetFunction("decimals").CallAsync<int>();
                    TotalSupply = await tusdContract.GetFunction("totalSupply").CallAsync<BigInteger>();
                    break;

                case "usdc":
                    var usdcContractABI = File.ReadAllText(@"c:\Apps\Test\Contracts\ERC20Tokens\USDC_ABI.json").Trim();
                    var usdcContract = this.ethereumService.GetContract(usdcContractABI, Erc20TokenModel.UsdcContractAddr);
                    //var usdcNameAwaiter = usdcContract.GetFunction("name").CallAsync<string>().GetAwaiter();
                    //var usdcSymbolAwaiter = usdcContract.GetFunction("symbol").CallAsync<string>().GetAwaiter();
                    //var usdcDecimalsAwaiter = usdcContract.GetFunction("decimals").CallAsync<int>().GetAwaiter();
                    //TotalSupply = await usdcContract.GetFunction("totalSupply").CallAsync<BigInteger>();

                    //usdcNameAwaiter.OnCompleted(() =>
                    //{
                    //    Name = usdcNameAwaiter.GetResult();
                    //});

                    //usdcSymbolAwaiter.OnCompleted(() =>
                    //{
                    //    Symbol = usdcSymbolAwaiter.GetResult();
                    //});

                    //usdcDecimalsAwaiter.OnCompleted(() =>
                    //{
                    //    Decimals = usdcDecimalsAwaiter.GetResult();
                    //});
                    break;

                case "bnb":
                    var bnbContractABI = File.ReadAllText(@"c:\Apps\Test\Contracts\ERC20Tokens\BNB_ABI.json").Trim();
                    var bnbContract = this.ethereumService.GetContract(bnbContractABI, Erc20TokenModel.BnbContractAddr);

                    FullName = await bnbContract.GetFunction("name").CallAsync<string>();
                    Symbol = await bnbContract.GetFunction("symbol").CallAsync<string>();
                    Decimals = await bnbContract.GetFunction("decimals").CallAsync<int>();
                    TotalSupply = await bnbContract.GetFunction("totalSupply").CallAsync<BigInteger>();
                    break;

            }
        }

    }
}
