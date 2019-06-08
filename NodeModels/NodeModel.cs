using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
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

        Timer fastQueryTimer;
        Timer slowQueryTimer;

        public NodeModel(INodeService nodeService, bool getAcctData)
        {
            this.ethereumService = nodeService;
            Status = NodeStatus.Connected;
            this.accounts = new ObservableCollection<AccountModel>();

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
    }
}
