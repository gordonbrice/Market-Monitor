using KeyStore;
using MVVMSupport;
using NodeModels;
using NodeServices;
using Utilities;

namespace NodeMonitor.WPF
{
    public class MainWindowViewModel : ViewModelBase
    {
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

        public MainWindowViewModel()
        {
            var passwordDlg = new PasswordDialog();
            if (passwordDlg.ShowDialog() == true)
            {
                var password1 = passwordDlg.Password1;
                var password2 = passwordDlg.Password2;
                var keyStoreAwaiter = KeyStore.GetApiKeys(password1, password2).GetAwaiter();

                keyStoreAwaiter.OnCompleted(() =>
                {
                    var key = KeyStore.InfuraMainnetKey;
                    var apiUrl = $"https://mainnet.infura.io/v3/{KeyStore.InfuraMainnetKey}";

                    Infura = new NodeModel(string.IsNullOrEmpty(KeyStore.InfuraMainnetKey) ? new EthereumNodeService("Infura", $"https://mainnet.infura.io") : new EthereumNodeService("Infura", apiUrl));

                });
            }

            //Infura = new NodeModel(new EthereumNodeService("Infura", "https://mainnet.infura.io"));
            //Local = new NodeModel(new EthereumNodeService("Local", "http://localhost:8545"), true, true);
            //Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8546")));
        }

    }
}
