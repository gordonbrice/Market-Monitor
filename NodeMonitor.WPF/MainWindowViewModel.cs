using KeyStore;
using MVVMSupport;
using NodeModels;
using NodeServices;
using System.Configuration;
using Utilities;

namespace NodeMonitor.WPF
{
    public class MainWindowViewModel : ViewModelBase
    {
        MongoAtlasStore store = new MongoAtlasStore();
        NodeModel infura;
        NodeModel alchemy;
        NodeModel chainstack;
        NodeModel getBlock;
        NodeModel myCrypto;
        NodeModel pocketNetwork;
        NodeModel cloudFlare;
        NodeModel myEtherWallet;
        NodeModel avado;
        NodeModel quickNode;
        NodeModel anyBlock;

        public NodeModel Infura
        {
            get
            {
                return this.infura;
            }
            private set
            {
                this.infura = value;
                OnPropertyChanged("Infura");
            }
        }
        public NodeModel QuickNode
        {
            get
            {
                return this.quickNode;
            }
            private set
            {
                this.quickNode = value;
                OnPropertyChanged("QuickNode");
            }
        }
        public NodeModel Alchemy
        {
            get
            {
                return this.alchemy;
            }
            private set
            {
                this.alchemy = value;
                OnPropertyChanged("Alchemy");
            }
        }
        public NodeModel GetBlock
        {
            get
            {
                return this.getBlock;
            }
            private set
            {
                this.getBlock = value;
                OnPropertyChanged("GetBlock");
            }
        }
        public NodeModel MyCrypto
        {
            get
            {
                return this.myCrypto;
            }
            private set
            {
                this.myCrypto = value;
                OnPropertyChanged("MyCrypto");
            }
        }
        public NodeModel PocketNetwork
        {
            get
            {
                return this.pocketNetwork;
            }
            private set
            {
                this.pocketNetwork = value;
                OnPropertyChanged("PocketNetwork");
            }
        }
        public NodeModel CloudFlare
        {
            get
            {
                return this.cloudFlare;
            }
            private set
            {
                this.cloudFlare = value;
                OnPropertyChanged("CloudFlare");
            }
        }
        public NodeModel MyEtherWallet
        {
            get
            {
                return this.myEtherWallet;
            }
            private set
            {
                this.myEtherWallet = value;
                OnPropertyChanged("MyEtherWallet");
            }
        }
        public NodeModel Chainstack
        {
            get
            {
                return this.chainstack;
            }
            private set
            {
                this.chainstack = value;
                OnPropertyChanged("Chainstack");
            }
        }
        public NodeModel Avado
        {
            get
            {
                return this.avado;
            }
            private set
            {
                this.avado = value;
                OnPropertyChanged("Avado");
            }
        }
        public NodeModel AnyBlock
        {
            get
            {
                return this.anyBlock;
            }
            private set
            {
                this.anyBlock = value;
                OnPropertyChanged("AnyBlock");
            }
        }
        public MainWindowViewModel()
        {
       
            var passwordDlg = new PasswordDialog();
            if (passwordDlg.ShowDialog() == true)
            {
                var password1 = passwordDlg.Password1;
                var password2 = passwordDlg.Password2;

                store.LogIn(password1, password2);
                store.GetApiKeys();

                var fastQueryInterval = 15;
                var slowQueryInterval = 3600;

                Infura = new NodeModel(string.IsNullOrEmpty(store.InfuraMainnetKey) ? new EthereumNodeService("Infura", $"https://mainnet.infura.io") : new EthereumNodeService("Infura", store.InfuraMainnetKey), false, false, fastQueryInterval, slowQueryInterval);
                Infura.Error += Node_Error;
                Infura.TotalErrors = store.GetTotalErrorCountFor(Infura.EthereumServiceName);
                Infura.ErrorsLastWeek = store.GetLastWeekErrorCountFor(Infura.EthereumServiceName);
                Infura.ErrorsLast24Hours = store.GetLast24HourErrorCountFor(Infura.EthereumServiceName);
                Infura.ErrorsLastHour = store.GetLastHourErrorCountFor(Infura.EthereumServiceName);

                if (!string.IsNullOrEmpty(store.AlchemyMainnetKey))
                {
                    Alchemy = new NodeModel(new EthereumNodeService("Alchemy", store.AlchemyMainnetKey), false, false, fastQueryInterval, slowQueryInterval);
                    Alchemy.Error += Node_Error;
                }

                if(!string.IsNullOrEmpty(store.GetBlockKey))
                {
                    GetBlock = new NodeModel(new EthereumNodeService("GetBlock", store.GetBlockKey), false, false, fastQueryInterval, slowQueryInterval);
                    GetBlock.Error += Node_Error;
                }

                MyCrypto = new NodeModel(new EthereumNodeService("MyCrypto", "https://api.mycryptoapi.com/eth"), false, false, fastQueryInterval, slowQueryInterval);
                MyCrypto.Error += Node_Error;
                PocketNetwork = new NodeModel(new EthereumNodeService("PocketNetwork", "https://eth-mainnet.gateway.pokt.network/v1/5f3453978e354ab992c4da79"), false, false, fastQueryInterval, slowQueryInterval);
                PocketNetwork.Error += Node_Error;
                CloudFlare = new NodeModel(new EthereumNodeService("CloudFlare", "https://cloudflare-eth.com/"), false, false, fastQueryInterval, slowQueryInterval);
                CloudFlare.Error += Node_Error;
                Avado = new NodeModel(new EthereumNodeService("Avado", "https://mainnet.eth.cloud.ava.do/"), false, false, fastQueryInterval, slowQueryInterval);
                Avado.Error += Node_Error;
                MyEtherWallet = new NodeModel(new EthereumNodeService("MyEtherWallet", "https://nodes.mewapi.io/rpc/eth"), false, false, fastQueryInterval, slowQueryInterval);
                MyEtherWallet.Error += Node_Error;
                QuickNode = new NodeModel(new EthereumNodeService("QuickNode", store.QuickNode), false, false, fastQueryInterval, slowQueryInterval);
                QuickNode.Error += Node_Error;

                if (!string.IsNullOrEmpty(store.ChainstackEth1Node1Key))
                {
                    Chainstack = new NodeModel(new EthereumNodeService("Chainstack", store.ChainstackEth1Node1Key), false, false, fastQueryInterval, slowQueryInterval);
                    Chainstack.Error += Node_Error;
                }

                if (!string.IsNullOrEmpty(store.AnyBlockMainnetKey))
                {
                    AnyBlock = new NodeModel(new EthereumNodeService("AnyBlock", store.AnyBlockMainnetKey), false, false, fastQueryInterval * 10, slowQueryInterval * 10);
                    AnyBlock.Error += Node_Error;
                }
            }

            //Infura = new NodeModel(new EthereumNodeService("Infura", "https://mainnet.infura.io"));
            //Local = new NodeModel(new EthereumNodeService("Local", "http://localhost:8545"), true, true);
            //Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8546")));
        }

        private void Node_Error(object sender, NodeErrorEventArgs e)
        {
            this.store.Log(e.Name, e.Message).Wait();

            
        }
    }
}
