﻿using MVVMSupport;
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
        NotDefined,
        Disconnected,
        Connected,
        Synching,
        Working,
        Error
    }
    public class NodeModel : ViewModelBase
    {
        IExecutionClientService ethereumService;

        public event EventHandler<NodeErrorEventArgs> Error;
        public event EventHandler<SlowQuertEventArgs> SlowQueryComplete;

        int totalErrors;
        public int TotalErrors
        {
            get { return this.totalErrors; }
            set
            {
                this.totalErrors = value;
                OnPropertyChanged("TotalErrors");
            }
        }

        int errorsLastWeek;
        public int ErrorsLastWeek
        {
            get { return this.errorsLastWeek; }
            set
            {
                this.errorsLastWeek = value;
                OnPropertyChanged("ErrorsLastWeek");
            }
        }

        int errorsLast24Hours;
        public int ErrorsLast24Hours
        {
            get { return this.errorsLast24Hours; }
            set
            {
                this.errorsLast24Hours = value;
                OnPropertyChanged("ErrorsLast24Hours");
            }
        }

        int errorsLastHour;
        public int ErrorsLastHour
        {
            get { return this.errorsLastHour; }
            set
            {
                this.errorsLastHour = value;
                OnPropertyChanged("ErrorsLastHour");
            }
        }

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

        int fastQueryInterval;
        public int FastQueryInterval
        {
            get { return this.fastQueryInterval; }
            private set
            {
                this.fastQueryInterval = value;
                OnPropertyChanged("FastQueryInterval");
            }
        }

        int slowQueryInterval;
        public int SlowQueryInterval
        {
            get { return this.slowQueryInterval; }
            set
            {
                this.slowQueryInterval = value;
                OnPropertyChanged("SlowQueryInterval");
            }
        }

        NodeStatus previousStatus = NodeStatus.NotDefined;

        NodeStatus status = NodeStatus.NotDefined;
        public NodeStatus Status
        {
            get { return status; }
            set
            {
                if(this.status == NodeStatus.NotDefined || this.status == NodeStatus.Connected)
                {
                    this.previousStatus = value;
                }

                this.status = value;
                OnPropertyChanged("Status");
                StatusStr = this.status.ToString();

                if(this.status != this.previousStatus)
                {
                    OnError(EthereumServiceName, this.statusStr);
                    this.previousStatus = this.status;
                }
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

        string statusDetail;
        public string StatusDetail
        {
            get
            {
                return this.statusDetail;
            }

            private set
            {
                this.statusDetail = value;
                OnPropertyChanged("StatusDetail");
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

        string clientVersion;
        public string ClientVersion
        {
            get { return this.clientVersion; }
            set
            {
                this.clientVersion = value;
                OnPropertyChanged("ClientVersion");
            }
        }

        string chainId;
        public string ChainId
        {
            get { return this.chainId;  }
            set
            {
                this.chainId = value;
                OnPropertyChanged("ChainId");
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

        double averageResponseTime;
        public double AverageResponseTime
        {
            get { return this.averageResponseTime; }
            set
            {
                this.averageResponseTime = value;
                OnPropertyChanged("AverageResponseTime");
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
        #region Financial
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
        #endregion

        Timer fastQueryTimer;
        Timer slowQueryTimer;
        bool contractInteraction;

        public NodeModel(IExecutionClientService nodeService, bool getAcctData = false, bool contractInteraction = false, int fastQueryInt = 5, int slowQueryInt = 60)
        {
            this.ethereumService = nodeService;
            FastQueryInterval = fastQueryInt;
            SlowQueryInterval = slowQueryInt;
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

            AverageResponseTime = 0.0;

            this.fastQueryTimer = new Timer((s) =>
            {
                FastQueryNode();
            }, null, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, FastQueryInterval));

            this.slowQueryTimer = new Timer((s) =>
            {
                SlowQueryNode();
                OnSlowQueryComplete();
            }, null, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, SlowQueryInterval));

        }

        private void OnSlowQueryComplete()
        {
            if(SlowQueryComplete != null)
            {
                SlowQueryComplete(this, new SlowQuertEventArgs());
            }
        }
        private void FastQueryNode()
        {
            try
            {
                var beginTime = DateTime.Now;
                var syncingAwaiter = this.ethereumService.GetSyncing().GetAwaiter();

                syncingAwaiter.OnCompleted(() =>
                {
                    var responseTime = (DateTime.Now - beginTime).TotalSeconds;

                    if(AverageResponseTime == 0.0)
                    {
                        AverageResponseTime = responseTime;
                    }
                    else
                    {
                        AverageResponseTime = (AverageResponseTime + responseTime) / 2;
                    }

                    try
                    {
                        var result = syncingAwaiter.GetResult();

                        if(result != null)
                        {
                            IsSyncing = result.IsSyncing;
                            HighestBlock = result.HighestBlock?.Value.ToString();
                            StartingBlock = result.StartingBlock?.Value.ToString();
                            CurrentBlock = result.CurrentBlock?.Value.ToString();

                            if (string.IsNullOrEmpty(HighestBlock))
                            {
                                beginTime = DateTime.Now;

                                var highestBlockTaskAwaiter = this.ethereumService.GetHighestBlock().GetAwaiter();

                                highestBlockTaskAwaiter.OnCompleted(() =>
                                {
                                    responseTime = (DateTime.Now - beginTime).TotalSeconds;
                                    AverageResponseTime = (AverageResponseTime + responseTime) / 2;

                                    try
                                    {
                                        HighestBlock = highestBlockTaskAwaiter.GetResult().Value.ToString();
                                    }
                                    catch (Exception x)
                                    {
                                        Status = NodeStatus.Error;
                                        StatusDetail = x.Message;
                                        OnError(this.EthereumServiceName, x.Message);
                                    }
                                });
                            }
                        }
                        else
                        {
                            Status = NodeStatus.Error;
                        }
                    }
                    catch(Exception e)
                    {
                        Status = NodeStatus.Error;
                        StatusDetail = e.Message;
                        OnError(this.EthereumServiceName, e.Message);
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
                    try
                    {
                        ProtocolVersion = protocolVerTaskAwaiter.GetResult();
                    }
                    catch(Exception)
                    {
                        ProtocolVersion = "N/A";
                    }
                });

                var clientVersionAwaiter = this.ethereumService.GetClientVersion().GetAwaiter();

                clientVersionAwaiter.OnCompleted(() =>
                {
                    try
                    {
                        ClientVersion = clientVersionAwaiter.GetResult();
                    }
                    catch (Exception cvx)
                    {
                        Status = NodeStatus.Error;
                        StatusDetail = cvx.Message;
                        OnError(this.EthereumServiceName, cvx.Message);
                    }
                });

                var chainIdAwaiter = this.ethereumService.GetChainId().GetAwaiter();

                chainIdAwaiter.OnCompleted(() =>
                {
                    try
                    {
                        ChainId = chainIdAwaiter.GetResult().Value.ToString();
                    }
                    catch(Exception x)
                    {
                        Status = NodeStatus.Error;
                        StatusDetail = x.Message;
                        OnError(this.EthereumServiceName, x.Message);
                    }
                });

                foreach(var account in Accounts)
                {
                    account.GetBalance();
                }
            }
            catch (Exception e)
            {
                Status = NodeStatus.Error;
                StatusDetail = e.Message;
                OnError(this.EthereumServiceName, e.Message);
            }
        }

        private void OnError(string name, string message)
        {
            if(Error != null)
            {
                Error(this, new NodeErrorEventArgs
                {
                    Name = name,
                    Message = message
                });
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
