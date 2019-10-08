using MVVMSupport;
using Nethereum.Contracts;
using NodeServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;

namespace NodeModels
{
    public enum NodeStatus
    {
        Disconnected,
        Connected,
        Synching,
        Working,
        Error
    }
    public class NodeModel : ViewModelBase
    {
        INodeService ethereumService;

        string ethereumServiceName;
        public string EthereumServiceName
        {
            get { return this.ethereumServiceName; }
            set
            {
                this.ethereumServiceName = value;
                OnPropertyChanged("EthereumServiceName");
            }
        }

        NodeStatus status = NodeStatus.Disconnected;

        public NodeStatus Status
        {
            get { return status; }
            set
            {
                this.status = value;
                OnPropertyChanged("Status");
                StatusStr = this.status.ToString();
            }
        }

        string statusStr;
        public string StatusStr
        {
            get
            {
                return this.statusStr;
            }

            private set
            {
                this.statusStr = value;
                OnPropertyChanged("StatusStr");
            }
        }

        string protocolVersion;
        public string ProtocolVersion
        {
            get { return this.protocolVersion; }
            set
            {
                this.protocolVersion = value;
                OnPropertyChanged("ProtocolVersion");
            }
        }

        string highestBlock;
        public string HighestBlock
        {
            get { return this.highestBlock; }
            set
            {
                this.highestBlock = value;
                OnPropertyChanged("HighestBlock");
            }
        }

        string startingBlock;
        public string StartingBlock
        {
            get { return this.startingBlock; }
            set
            {
                this.startingBlock = value;
                OnPropertyChanged("StartingBlock");
            }
        }

        string currentBlock;
        public string CurrentBlock
        {
            get { return this.currentBlock; }
            set
            {
                this.currentBlock = value;
                OnPropertyChanged("CurrentBlock");
            }
        }

        bool isSyncing;
        public bool IsSyncing
        {
            get { return this.isSyncing; }
            set
            {
                this.isSyncing = value;
                OnPropertyChanged("IsSyncing");
                OnPropertyChanged("SyncVisibility");
                Status = this.IsSyncing ? NodeStatus.Synching : NodeStatus.Working;
                Syncing = this.isSyncing ? "Yes" : "No";
            }
        }

        public string SyncVisibility
        {
            get
            {
                if (this.IsSyncing)
                {
                    return "Visible";
                }
                else
                {
                    return "Collapsed";
                }
            }
        }
        string syncing;
        public string Syncing
        {
            get { return this.syncing ; }
            set
            {
                this.syncing = value;
                OnPropertyChanged("Syncing");
            }
        }

        #region Uniswap
        #region Contracts
        Contract uniswapFactoryContract;
        Contract uniswapDaiExchContract;
        Contract uniswapUsdcExchContract;
        Contract uniswapCdaiExchContract;
        Contract uniswapTusdExchContract;
        #endregion

        #region Addresses
        #region Mainnet
        string mainNetFactoryAddr = "0xc0a47dFe034B400B47bDaD5FecDa2621de6c4d95";
        #endregion
        #endregion
        #endregion
        #region Uniswap Exchanges
        string uniswapFactoryABI;
        string uniswapExchangeABI;
        string daiExchAddr = null;
        string usdcExchAddr = null;
        string cdaiExchAddr = null;
        string tusdExchAddr = null;
        #endregion

        ObservableCollection<AccountModel> accounts;
        public ObservableCollection<AccountModel> Accounts
        {
            get
            {
                return this.accounts;
            }
            private set
            {
                this.accounts = value;
            }
        }

        ObservableCollection<PriceModel> prices;
        public ObservableCollection<PriceModel> Prices
        {
            get
            {
                return this.prices;
            }
            private set
            {
                this.prices = value;
            }
        }

        ObservableCollection<Erc20TokenModel> tokens;
        public ObservableCollection<Erc20TokenModel> Tokens
        {
            get
            {
                return this.tokens;
            }
            private set
            {
                this.tokens = value;
            }
        }

        Timer fastQueryTimer;
        Timer slowQueryTimer;
        bool contractInteraction;

        public NodeModel(INodeService nodeService, bool getAcctData = false, bool contractInteraction = false)
        {
            this.ethereumService = nodeService;
            EthereumServiceName = nodeService.Name;
            Status = NodeStatus.Connected;
            this.accounts = new ObservableCollection<AccountModel>();
            this.prices = new ObservableCollection<PriceModel>();
            this.tokens = new ObservableCollection<Erc20TokenModel>();
            this.contractInteraction = contractInteraction;

            if(contractInteraction)
            {
                AddTokens();
            }

            if (getAcctData)
            {
                var addressStr = File.ReadAllText(@"c:\Apps\Test\eth.txt").Trim();
                var addresses = addressStr.Split('|');

                foreach (var address in addresses)
                {
                    if (!string.IsNullOrEmpty(address.Trim()))
                    {
                        var addr = address.Split(',');
                        var account = new AccountModel(this.ethereumService, addr[1], addr[0]);

                        Accounts.Add(account);
                    }
                }
            }

            this.fastQueryTimer = new Timer((s) =>
            {
                FastQueryNode();
            }, null, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 5));

            this.slowQueryTimer = new Timer((s) =>
            {
                SlowQueryNode();
            }, null, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 60));

        }

        public void FastQueryNode()
        {
            try
            {
                var syncingAwaiter = this.ethereumService.GetSyncing().GetAwaiter();

                syncingAwaiter.OnCompleted(() =>
                {
                    try
                    {
                        var result = syncingAwaiter.GetResult();
                        IsSyncing = result.IsSyncing;
                        HighestBlock = result.HighestBlock?.Value.ToString();
                        StartingBlock = result.StartingBlock?.Value.ToString();
                        CurrentBlock = result.CurrentBlock?.Value.ToString();

                        if (string.IsNullOrEmpty(HighestBlock))
                        {
                            var highestBlockTaskAwaiter = this.ethereumService.GetHighestBlock().GetAwaiter();

                            highestBlockTaskAwaiter.OnCompleted(() =>
                            {
                                HighestBlock = highestBlockTaskAwaiter.GetResult().Value.ToString();
                            });
                        }
                    }
                    catch(Exception e)
                    {
                        Status = NodeStatus.Error;
                        //Log(e);
                    }
                });

                GetUniswapPrices();
            }
            catch (Exception e)
            {
                StatusStr = e.Message;
            }
        }

        public void SlowQueryNode()
        {
            try
            {
                var protocolVerTaskAwaiter = this.ethereumService.GetProtocolVersion().GetAwaiter();

                protocolVerTaskAwaiter.OnCompleted(() =>
                {
                    ProtocolVersion = protocolVerTaskAwaiter.GetResult();
                });

                foreach(var account in Accounts)
                {
                    account.GetBalance();
                }
            }
            catch (Exception e)
            {
                StatusStr = e.Message;
            }
        }

        private void AddTokens()
        {
            var dai = new Erc20TokenModel("DAI", this.ethereumService);

            dai.QueryProperties();
            Tokens.Add(dai);

            var bnb = new Erc20TokenModel("BNB", this.ethereumService);

            bnb.QueryProperties();
            Tokens.Add(bnb);

            var wbtc = new Erc20TokenModel("WBTC", this.ethereumService);

            wbtc.QueryProperties();
            Tokens.Add(wbtc);

            var usdt = new Erc20TokenModel("USDT", this.ethereumService);

            usdt.QueryProperties();
            Tokens.Add(usdt);

            //var usdc = new Erc20TokenModel("USDC", this.ethereumService);

            //usdc.QueryProperties();
            //Tokens.Add(usdc);

            var tusd = new Erc20TokenModel("TUSD", this.ethereumService);

            tusd.QueryProperties();
            Tokens.Add(tusd);

            var link = new Erc20TokenModel("LINK", this.ethereumService);

            link.QueryProperties();
            Tokens.Add(link);

            var mkr = new Erc20TokenModel("MKR", this.ethereumService);

            mkr.QueryProperties();
            Tokens.Add(mkr);

            var ht = new Erc20TokenModel("HT", this.ethereumService);

            ht.QueryProperties();
            Tokens.Add(ht);

            //var pax = new Erc20TokenModel("PAX", this.ethereumService);

            //pax.QueryProperties();
            //Tokens.Add(pax);
        }
        private void GetPrice(string ethTokenPair)
        {
            Function getEthToTokenInputPriceFunction = null;

            switch(ethTokenPair)
            {
                case "ETH-DAI":
                    getEthToTokenInputPriceFunction = this.uniswapDaiExchContract.GetFunction("getEthToTokenInputPrice");
                    break;

                case "ETH-USDC":
                    getEthToTokenInputPriceFunction = this.uniswapUsdcExchContract.GetFunction("getEthToTokenInputPrice");
                    break;

                case "ETH-TUSD":
                    getEthToTokenInputPriceFunction = this.uniswapTusdExchContract.GetFunction("getEthToTokenInputPrice");
                    break;

                default:
                    var price = new PriceModel
                    {
                        Name = ethTokenPair,
                        Token = null,
                        Market = "Unsupported",
                        Open = 0,
                        High = 0,
                        Low = 0,
                        Close = 0,
                        Time = DateTime.UtcNow
                    };

                    lock(Prices)
                    {
                        if(Prices.Contains(price))
                        {
                            var oldPriceIdx = Prices.IndexOf(price);

                            if (oldPriceIdx > -1)
                            {
                                Prices[oldPriceIdx].Time = DateTime.UtcNow;
                            }
                            else
                            {
                                Prices.Add(price);
                            }
                        }
                        else
                        {
                            Prices.Add(price);
                        }
                    }
                    return;
            }

            var ethPriceAwaiter = getEthToTokenInputPriceFunction.CallAsync<UInt64>(1).GetAwaiter();

            ethPriceAwaiter.OnCompleted(() =>
            {
                var ethPrice = ethPriceAwaiter.GetResult();
                var baseTokenName = ethTokenPair.Split('-')[1];
                Erc20TokenModel baseToken = null;

                foreach(var token in Tokens)
                {
                    if(token.Name == baseTokenName)
                    {
                        baseToken = token;
                        break;
                    }
                }

                if(baseToken == null)
                {
                    baseToken = new Erc20TokenModel(baseTokenName, this.ethereumService);
                    baseToken.QueryProperties();
                    Tokens.Add(baseToken);
                }

                var price = new PriceModel
                {
                    Name = ethTokenPair,
                    Token = baseToken,
                    Market = "Uniswap",
                    Open = ethPrice,
                    High = ethPrice,
                    Low = ethPrice,
                    Close = ethPrice,
                    Time = DateTime.UtcNow
                };

                lock (Prices)
                {
                    if (Prices.Contains(price))
                    {
                        var oldPriceIdx = Prices.IndexOf(price);

                        if (oldPriceIdx > -1)
                        {
                            Prices[oldPriceIdx].High = ethPrice > Prices[oldPriceIdx].High ? ethPrice : Prices[oldPriceIdx].High;
                            Prices[oldPriceIdx].Low = ethPrice < Prices[oldPriceIdx].Low ? ethPrice : Prices[oldPriceIdx].Low;
                            Prices[oldPriceIdx].Close = ethPrice;
                            Prices[oldPriceIdx].Time = DateTime.UtcNow;
                        }
                        else
                        {
                            Prices.Add(price);
                        }
                    }
                    else
                    {
                        Prices.Add(price);
                    }
                }
            });
        }

        private void GetUniswapPrices()
        {
            if (this.contractInteraction)
            {
                this.uniswapFactoryABI = string.IsNullOrEmpty(this.uniswapFactoryABI) ? File.ReadAllText(@"c:\Apps\Test\Contracts\Uniswap\FactoryABI.json").Trim() : this.uniswapFactoryABI;
                this.uniswapExchangeABI = string.IsNullOrEmpty(this.uniswapExchangeABI) ? File.ReadAllText(@"c:\Apps\Test\Contracts\Uniswap\ExchangeABI.json").Trim() : this.uniswapExchangeABI;
                var tokenPairs = new List<string>();

                tokenPairs.Add("ETH-DAI");
                //tokenPairs.Add("ETH-USDC");
                //tokenPairs.Add("ETH-cDAI");
                //tokenPairs.Add("ETH-TUSD");
                //tokenPairs.Add("DAI-USDC");
                //tokenPairs.Add("DAI-cDAI");

                foreach(var tokenPair in tokenPairs)
                {
                    switch(tokenPair)
                    {
                        case "ETH-DAI":
                            if (this.uniswapDaiExchContract == null)
                            {
                                if (this.uniswapFactoryContract == null)
                                {
                                    this.uniswapFactoryContract = this.ethereumService.GetContract(this.uniswapFactoryABI, this.mainNetFactoryAddr);
                                }

                                var getExchangeFunction = this.uniswapFactoryContract.GetFunction("getExchange");
                                var daiExchangeAwaiter = getExchangeFunction.CallAsync<string>(Erc20TokenModel.DaiContractAddr).GetAwaiter();

                                daiExchangeAwaiter.OnCompleted(() =>
                                {
                                    this.daiExchAddr = daiExchangeAwaiter.GetResult();
                                    this.uniswapDaiExchContract = this.ethereumService.GetContract(this.uniswapExchangeABI, this.daiExchAddr);
                                    GetPrice(tokenPair);
                                });
                            }
                            else
                            {
                                GetPrice(tokenPair);
                            }
                            break;

                        case "ETH-TUSD":
                            if (this.uniswapTusdExchContract == null)
                            {
                                if (this.uniswapFactoryContract == null)
                                {
                                    this.uniswapFactoryContract = this.ethereumService.GetContract(this.uniswapFactoryABI, this.mainNetFactoryAddr);
                                }

                                var getExchangeFunction = this.uniswapFactoryContract.GetFunction("getExchange");
                                var tusdExchangeAwaiter = getExchangeFunction.CallAsync<string>(Erc20TokenModel.TusdContractAddr).GetAwaiter();

                                tusdExchangeAwaiter.OnCompleted(() =>
                                {
                                    this.tusdExchAddr = tusdExchangeAwaiter.GetResult();
                                    this.uniswapTusdExchContract = this.ethereumService.GetContract(this.uniswapExchangeABI, this.tusdExchAddr);
                                    GetPrice(tokenPair);
                                });
                            }
                            else
                            {
                                GetPrice(tokenPair);
                            }
                            break;

                        case "ETH-USDC":
                            if (this.uniswapUsdcExchContract == null)
                            {
                                if (this.uniswapFactoryContract == null)
                                {
                                    this.uniswapFactoryContract = this.ethereumService.GetContract(this.uniswapFactoryABI, this.mainNetFactoryAddr);
                                }

                                var getExchangeFunction = this.uniswapFactoryContract.GetFunction("getExchange");
                                var usdcExchangeAwaiter = getExchangeFunction.CallAsync<string>(Erc20TokenModel.UsdcContractAddr).GetAwaiter();

                                usdcExchangeAwaiter.OnCompleted(() =>
                                {
                                    this.usdcExchAddr = usdcExchangeAwaiter.GetResult();
                                    this.uniswapUsdcExchContract = this.ethereumService.GetContract(this.uniswapExchangeABI, this.usdcExchAddr);
                                    GetPrice(tokenPair);
                                });
                            }
                            else
                            {
                                GetPrice(tokenPair);
                            }
                            break;


                        default:
                            GetPrice(tokenPair);
                            break;
                    }
                }


            }
        }
    }
}
