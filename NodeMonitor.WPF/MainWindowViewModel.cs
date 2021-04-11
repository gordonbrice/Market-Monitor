using KeyStore;
using MVVMSupport;
using NodeModels;
using NodeServices;
using System.Collections.ObjectModel;
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
        NodeModel archiveNode;
        ObservableCollection<NodeModel> nodes;

        public ObservableCollection<NodeModel> Nodes
        {
            get { return this.nodes; }
            private set
            {
                this.nodes = value;
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

                Nodes = new ObservableCollection<NodeModel>();

                var node = new NodeModel(string.IsNullOrEmpty(store.InfuraMainnetKey) ? new EthereumNodeService("Infura", $"https://mainnet.infura.io") : new EthereumNodeService("Infura", store.InfuraMainnetKey), false, false, fastQueryInterval, slowQueryInterval);
                
                node.Error += Node_Error;
                //node.TotalErrors = store.GetTotalErrorCountFor(node.EthereumServiceName);
                Nodes.Add(node);
                //Infura.ErrorsLastWeek = store.GetLastWeekErrorCountFor(Infura.EthereumServiceName);
                //Infura.ErrorsLast24Hours = store.GetLast24HourErrorCountFor(Infura.EthereumServiceName);
                //Infura.ErrorsLastHour = store.GetLastHourErrorCountFor(Infura.EthereumServiceName);

                if (!string.IsNullOrEmpty(store.AlchemyMainnetKey))
                {
                    node = new NodeModel(new EthereumNodeService("Alchemy", store.AlchemyMainnetKey), false, false, fastQueryInterval, slowQueryInterval);
                    node.Error += Node_Error;
                    Nodes.Add(node);
                }

                if (!string.IsNullOrEmpty(store.GetBlockKey))
                {
                    node = new NodeModel(new EthereumNodeService("GetBlock", store.GetBlockKey), false, false, fastQueryInterval, slowQueryInterval);
                    node.Error += Node_Error;
                    Nodes.Add(node);
                }

                node = new NodeModel(new EthereumNodeService("MyCrypto", "https://api.mycryptoapi.com/eth"), false, false, fastQueryInterval, slowQueryInterval);
                node.Error += Node_Error;
                Nodes.Add(node);
                node = new NodeModel(new EthereumNodeService("PocketNetwork", "https://eth-mainnet.gateway.pokt.network/v1/5f3453978e354ab992c4da79"), false, false, fastQueryInterval, slowQueryInterval);
                node.Error += Node_Error;
                Nodes.Add(node);
                node = new NodeModel(new EthereumNodeService("CloudFlare", "https://cloudflare-eth.com/"), false, false, fastQueryInterval, slowQueryInterval);
                node.Error += Node_Error;
                Nodes.Add(node);
                node = new NodeModel(new EthereumNodeService("Avado", "https://mainnet.eth.cloud.ava.do/"), false, false, fastQueryInterval, slowQueryInterval);
                node.Error += Node_Error;
                Nodes.Add(node);
                node = new NodeModel(new EthereumNodeService("MyEtherWallet", "https://nodes.mewapi.io/rpc/eth"), false, false, fastQueryInterval, slowQueryInterval);
                node.Error += Node_Error;
                Nodes.Add(node);
                node = new NodeModel(new EthereumNodeService("QuickNode", store.QuickNode), false, false, fastQueryInterval, slowQueryInterval);
                node.Error += Node_Error;
                Nodes.Add(node);

                if (!string.IsNullOrEmpty(store.ChainstackEth1Node1Key))
                {
                    node = new NodeModel(new EthereumNodeService("Chainstack", store.ChainstackEth1Node1Key), false, false, fastQueryInterval, slowQueryInterval);
                    node.Error += Node_Error;
                    Nodes.Add(node);
                }

                if (!string.IsNullOrEmpty(store.AnyBlockMainnetKey))
                {
                    node = new NodeModel(new EthereumNodeService("AnyBlock", store.AnyBlockMainnetKey), false, false, fastQueryInterval * 10, slowQueryInterval * 10);
                    node.Error += Node_Error;
                    Nodes.Add(node);
                }

                if (!string.IsNullOrEmpty(store.ArchiveNodeKey))
                {
                    node = new NodeModel(new EthereumNodeService("ArchiveNode", store.ArchiveNodeKey), false, false, fastQueryInterval * 10, slowQueryInterval * 10);
                    node.Error += Node_Error;
                    Nodes.Add(node);
                }

            }

            //Infura = new NodeModel(new EthereumNodeService("Infura", "https://mainnet.infura.io"));
            //Local = new NodeModel(new EthereumNodeService("Local", "http://localhost:8545"), true, true);
            //Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8546")));
        }

        private void Node_Error(object sender, NodeErrorEventArgs e)
        {
            //this.store.Log(e.Name, e.Message).Wait();

            
        }
    }
}
