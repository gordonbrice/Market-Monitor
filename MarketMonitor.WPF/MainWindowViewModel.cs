
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
                var password1 = passwordDlg.PasswordPwBox1.Password;
                var password2 = passwordDlg.PasswordPwBox2.Password;
                var keyStoreAwaiter = KeyStore.GetApiKeys(password1, password2).GetAwaiter();

                keyStoreAwaiter.OnCompleted(() =>
                {
                    var apiUrl = $"https://mainnet.infura.io/v3/{KeyStore.InfuraMainnetKey}";

                    Infura = new NodeModel(string.IsNullOrEmpty(KeyStore.InfuraMainnetKey) ? new EthereumNodeService("Infura", $"https://mainnet.infura.io") : new EthereumNodeService("Infura", apiUrl));
                });
            }

            Infura = new NodeModel(new EthereumNodeService("Infura", "https://mainnet.infura.io"));
            Local = new NodeModel(new EthereumNodeService("Local", "http://localhost:8545"), true, true);
            //Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8546")));
        }

        private CloudStore KeyStore = new CloudStore();
        public NodeModel Infura { get; private set; }
        public NodeModel Local { get; private set; }
    }
}
