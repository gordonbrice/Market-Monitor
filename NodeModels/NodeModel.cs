using Nethereum.Contracts;
using NodeServices;
using System;
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
                Status = this.IsSyncing ? NodeStatus.Synching : NodeStatus.Working;
                Syncing = this.isSyncing ? "Yes" : "No";
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
        #endregion

        #region Addresses
        #region Mainnet
        string mainNetFactoryAddr = "0xc0a47dFe034B400B47bDaD5FecDa2621de6c4d95";
        #endregion
        #endregion
        #endregion
        #region Uniswap Exchanges
        string daiExchAddr = null;

        #endregion

        #region Token Contracts
        string daiContractAddr = "0x89d24A6b4CcB1B6fAA2625fE562bDD9a23260359";

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

        Timer fastQueryTimer;
        Timer slowQueryTimer;
        bool contractInteraction;

        public NodeModel(INodeService nodeService, bool getAcctData = false, bool contractInteraction = false)
        {
            this.ethereumService = nodeService;
            Status = NodeStatus.Connected;
            this.accounts = new ObservableCollection<AccountModel>();
            
            this.prices = new ObservableCollection<PriceModel>();
            this.contractInteraction = contractInteraction;

            if(getAcctData)
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
                });

                GetPrice();
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

        private void GetEthDaiPrice()
        {
            var getEthToTokenInputPriceFunction = this.uniswapDaiExchContract.GetFunction("getEthToTokenInputPrice");
            var ethPriceAwaiter = getEthToTokenInputPriceFunction.CallAsync<UInt64>(1).GetAwaiter();

            ethPriceAwaiter.OnCompleted(() =>
            {
                var ethPrice = ethPriceAwaiter.GetResult();

                if (this.prices.Count == 0)
                {
                    var price = new PriceModel
                    {
                        Name = "ETH-DAI",
                        Market = "Uniswap",
                        Open = ethPrice,
                        High = ethPrice,
                        Low = ethPrice,
                        Close = ethPrice,
                        Time = DateTime.UtcNow
                    };

                    lock (Prices)
                    {
                        Prices.Add(price);
                    }
                }
                else
                {
                    foreach (var price in Prices)
                    {
                        if (price.Name == "ETH-DAI")
                        {
                            lock (Prices)
                            {
                                price.High = ethPrice > price.High ? ethPrice : price.High;
                                price.Low = ethPrice < price.Low ? ethPrice : price.Low;
                                price.Close = ethPrice;
                                price.Time = DateTime.UtcNow;
                            }
                        }

                        break;
                    }
                }
            });
        }

        private void GetPrice()
        {
            if (this.contractInteraction)
            {
                if (this.uniswapDaiExchContract == null)
                {
                    if (this.uniswapFactoryContract == null)
                    {
                        var factoryABI = File.ReadAllText(@"c:\Apps\Test\Contracts\Uniswap\FactoryABI.json").Trim();
                        this.uniswapFactoryContract = this.ethereumService.GetContract(factoryABI, this.mainNetFactoryAddr);
                    }

                    var getExchangeFunction = this.uniswapFactoryContract.GetFunction("getExchange");
                    var daiExchangeAwaiter = getExchangeFunction.CallAsync<string>(this.daiContractAddr).GetAwaiter();

                    daiExchangeAwaiter.OnCompleted(() =>
                    {
                        this.daiExchAddr = daiExchangeAwaiter.GetResult();
                        var exchangeABI = File.ReadAllText(@"c:\Apps\Test\Contracts\Uniswap\ExchangeABI.json").Trim();
                        this.uniswapDaiExchContract = this.ethereumService.GetContract(exchangeABI, this.daiExchAddr);
                        GetEthDaiPrice();
                    });
                }
                else
                {
                    GetEthDaiPrice();
                }
            }
        }
    }
}
