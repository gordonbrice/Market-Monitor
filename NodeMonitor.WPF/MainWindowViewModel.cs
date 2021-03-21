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

                if (!string.IsNullOrEmpty(keyStore.ChainstackEth1Node1Key))
                {
                    //Chainstack = new NodeModel(new EthereumNodeService("Chainstack", keyStore.ChainstackEth1Node1Key));
                }
            }

            //Infura = new NodeModel(new EthereumNodeService("Infura", "https://mainnet.infura.io"));
            //Local = new NodeModel(new EthereumNodeService("Local", "http://localhost:8545"), true, true);
            //Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8546")));
        }

    }
}
