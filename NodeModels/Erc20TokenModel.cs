using Nethereum.Hex.HexTypes;
using NodeServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace NodeModels
{
    public class Erc20TokenModel : ViewModelBase, IEquatable<Erc20TokenModel>
    {
        #region Mainnet Token Contract Addresses
        public static string DaiContractAddr = "0x89d24A6b4CcB1B6fAA2625fE562bDD9a23260359";
        public static string UsdcContractAddr = "0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48";
        public static string cDaiContractAddr = "0xf5dce57282a584d2746faf1593d3121fcac444dc";
        public static string TusdContractAddr = "0x0000000000085d4780B73119b644AE5ecd22b376";
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

        public void QueryProperties()
        {
            switch (this.name.ToLower())
            {
                case "dai":
                    var daiContractABI = File.ReadAllText(@"c:\Apps\Test\Contracts\ERC20Tokens\DAI_ABI.json").Trim();

                    var daiContract = this.ethereumService.GetContract(daiContractABI, Erc20TokenModel.DaiContractAddr);
                    var daiNameAwaiter = daiContract.GetFunction("name").CallAsync<string>().GetAwaiter();
                    var daiSymbolAwaiter = daiContract.GetFunction("symbol").CallAsync<string>().GetAwaiter();
                    var daiDecimalsAwaiter = daiContract.GetFunction("decimals").CallAsync<int>().GetAwaiter();
                    var daiTotalSupplyAwaiter = daiContract.GetFunction("totalSupply").CallAsync<BigInteger>().GetAwaiter();

                    daiNameAwaiter.OnCompleted(() =>
                    {
                        Name = daiNameAwaiter.GetResult();
                    });

                    daiSymbolAwaiter.OnCompleted(() =>
                    {
                        Symbol = daiNameAwaiter.GetResult();
                    });

                    daiDecimalsAwaiter.OnCompleted(() =>
                    {
                        Decimals = daiDecimalsAwaiter.GetResult();
                    });

                    daiTotalSupplyAwaiter.OnCompleted(() =>
                    {
                        TotalSupply = daiTotalSupplyAwaiter.GetResult();
                    });
                    break;

                case "tusd":
                    var tusdContractABI = File.ReadAllText(@"c:\Apps\Test\Contracts\ERC20Tokens\TUSD_ABI.json").Trim();
                    var tusdContract = this.ethereumService.GetContract(tusdContractABI, Erc20TokenModel.DaiContractAddr);
                    //var tusdNameAwaiter = tusdContract.GetFunction("name").CallAsync<string>().GetAwaiter();
                    //var tusdSymbolAwaiter = tusdContract.GetFunction("symbol").CallAsync<string>().GetAwaiter();
                    //var tusdDecimalsAwaiter = tusdContract.GetFunction("decimals").CallAsync<int>().GetAwaiter();
                    var tusdTotalSupplyAwaiter = tusdContract.GetFunction("totalSupply").CallAsync<ulong>().GetAwaiter();

                    //tusdNameAwaiter.OnCompleted(() =>
                    //{
                    //    Name = tusdNameAwaiter.GetResult();
                    //});

                    //tusdSymbolAwaiter.OnCompleted(() =>
                    //{
                    //    Symbol = tusdNameAwaiter.GetResult();
                    //});

                    //tusdDecimalsAwaiter.OnCompleted(() =>
                    //{
                    //    Decimals = tusdDecimalsAwaiter.GetResult();
                    //});
                    tusdTotalSupplyAwaiter.OnCompleted(() =>
                    {
                        TotalSupply = tusdTotalSupplyAwaiter.GetResult();
                    });
                    break;

                case "usdc":
                    var usdcContractABI = File.ReadAllText(@"c:\Apps\Test\Contracts\ERC20Tokens\USDC_ABI.json").Trim();
                    var usdcContract = this.ethereumService.GetContract(usdcContractABI, Erc20TokenModel.UsdcContractAddr);
                    //var usdcNameAwaiter = usdcContract.GetFunction("name").CallAsync<string>().GetAwaiter();
                    //var usdcSymbolAwaiter = usdcContract.GetFunction("symbol").CallAsync<string>().GetAwaiter();
                    //var usdcDecimalsAwaiter = usdcContract.GetFunction("decimals").CallAsync<int>().GetAwaiter();

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
            }
        }

        public bool Equals(Erc20TokenModel other)
        {
            return Name == other.Name;
        }
    }
}
