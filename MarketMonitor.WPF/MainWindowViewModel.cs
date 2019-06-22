
using Nethereum.Web3;
using NodeModels;
using NodeServices;

namespace MarketMonitor.WPF
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            //Infura = new NodeModel(new EthereumNodeService(new Web3("https://mainnet.infura.io")));
            Infura = new NodeModel(new EthereumNodeService(new Web3("https://mainnet.infura.io/v3/07181aa6795e4560b22dce7ddc95a430")), false);
            Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8545")), true, true);
            //Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8546")));
        }
        public NodeModel Infura { get; private set; }
        public NodeModel Local { get; private set; }

        
    }
}
