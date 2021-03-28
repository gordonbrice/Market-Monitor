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
        MongoAtlasStore keyStore = new MongoAtlasStore();
        NodeModel infura;
        NodeModel alchemy;
        NodeModel chainstack;
        NodeModel getBlock;
        NodeModel myCrypto;
        NodeModel pocketNetwork;
        NodeModel cloudFlare;
        NodeModel myEtherWallet;
        NodeModel avado;

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

        public MainWindowViewModel()
        {
       
            var passwordDlg = new PasswordDialog();
            if (passwordDlg.ShowDialog() == true)
            {
                var password1 = passwordDlg.Password1;
                var password2 = passwordDlg.Password2;

                keyStore.LogIn(password1, password2);
                keyStore.GetApiKeys();
                Infura = new NodeModel(string.IsNullOrEmpty(keyStore.InfuraMainnetKey) ? new EthereumNodeService("Infura", $"https://mainnet.infura.io") : new EthereumNodeService("Infura", keyStore.InfuraMainnetKey));

                if(!string.IsNullOrEmpty(keyStore.AlchemyMainnetKey))
                {
                    Alchemy = new NodeModel(new EthereumNodeService("Alchemy", keyStore.AlchemyMainnetKey));
                }

                if(!string.IsNullOrEmpty(keyStore.GetBlockKey))
                {
                    GetBlock = new NodeModel(new EthereumNodeService("GetBlock", keyStore.GetBlockKey));
                }

                MyCrypto = new NodeModel(new EthereumNodeService("MyCrypto", "https://api.mycryptoapi.com/eth"));
                //PocketNetwork = new NodeModel(new EthereumNodeService("PocketNetwork", "https://eth-mainnet.gateway.pokt.network/v1/5f3453978e354ab992c4da79"));
                CloudFlare = new NodeModel(new EthereumNodeService("CloudFlare", "https://cloudflare-eth.com/"));
                //Avado = new NodeModel(new EthereumNodeService("Avado", "https://mainnet.eth.cloud.ava.do/"));
                //MyEtherWallet = new NodeModel(new EthereumNodeService("MyEtherWallet", "https://nodes.mewapi.io/rpc/eth"));

                //if (!string.IsNullOrEmpty(keyStore.ChainstackEth1Node1Key))
                //{
                //    Chainstack = new NodeModel(new EthereumNodeService("Chainstack", keyStore.ChainstackEth1Node1Key));
                //}
            }

            //Infura = new NodeModel(new EthereumNodeService("Infura", "https://mainnet.infura.io"));
            //Local = new NodeModel(new EthereumNodeService("Local", "http://localhost:8545"), true, true);
            //Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8546")));
        }

    }
}
