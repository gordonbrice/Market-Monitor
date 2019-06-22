
using KeyStore;
using Nethereum.Web3;
using NodeModels;
using NodeServices;

namespace MarketMonitor.WPF
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            var passwordDlg = new PasswordDialog();
            if (passwordDlg.ShowDialog() == true)
            {
                var password = passwordDlg.PasswordPwBox.Password;
                var keyStoreAwaiter = KeyStore.GetApiKeys(password).GetAwaiter();

                keyStoreAwaiter.OnCompleted(() =>
                {
                    Infura = new NodeModel(new EthereumNodeService(new Web3($"https://mainnet.infura.io/v3/{KeyStore.InfuraMainnetKey}")), false);
                });
            }

            //Infura = new NodeModel(new EthereumNodeService(new Web3("https://mainnet.infura.io")));
            Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8545")), true, true);
            //Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8546")));
        }

        private CloudStore KeyStore = new CloudStore();
        public NodeModel Infura { get; private set; }
        public NodeModel Local { get; private set; }
    }
}
