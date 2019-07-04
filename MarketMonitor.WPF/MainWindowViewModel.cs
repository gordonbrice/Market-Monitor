
using KeyStore;
using Nethereum.Web3;
using NodeModels;
using NodeServices;

namespace MarketMonitor.WPF
{
    public class MainWindowViewModel : ViewModelBase
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
                    var key = KeyStore.InfuraMainnetKey;
                    var apiUrl = $"https://mainnet.infura.io/v3/{KeyStore.InfuraMainnetKey}";

                    Infura = new NodeModel(string.IsNullOrEmpty(KeyStore.InfuraMainnetKey) ? new EthereumNodeService("Infura", $"https://mainnet.infura.io") : new EthereumNodeService("Infura", apiUrl));
                });
            }

            //Infura = new NodeModel(new EthereumNodeService("Infura", "https://mainnet.infura.io"));
            Local = new NodeModel(new EthereumNodeService("Local", "http://localhost:8545"), true, true);
            //Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8546")));
        }

        private CloudStore KeyStore = new CloudStore();
        NodeModel infura;
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
        public NodeModel Local { get; private set; }
    }
}
