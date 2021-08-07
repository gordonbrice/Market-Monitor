using KeyStore;
using MVVMSupport;
using NodeModels;
using NodeServices;
using System.Net.Http;
using WhaleAlert;

namespace MarketMonitor.WPF
{
    public class MainWindowViewModel : ViewModelBase
    {
        private MongoAtlasStore KeyStore = new MongoAtlasStore();
        NodeModel infura;
        HttpClient httpClient = null;

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
        public WhaleAlertModel WhaleAlert { get; private set; }

        public MainWindowViewModel()
        {
            var passwordDlg = new PasswordDialog();
            if (passwordDlg.ShowDialog() == true)
            {
                var password1 = passwordDlg.PasswordPwBox1.Password;
                var password2 = passwordDlg.PasswordPwBox2.Password;

                KeyStore.LogIn(password1, password2);
                KeyStore.GetApiKeys();

                var apiUrl = KeyStore.InfuraMainnetKey;

                httpClient = new HttpClient();
                Infura = new NodeModel(string.IsNullOrEmpty(KeyStore.InfuraMainnetKey) ? new EthereumNodeService("Infura", $"https://mainnet.infura.io", httpClient) : new EthereumNodeService("Infura", apiUrl, httpClient));

                var client = new HttpClient();

                WhaleAlert = new WhaleAlertModel(client, KeyStore.WhaleAlertKey);

            }

            //Infura = new NodeModel(new EthereumNodeService("Infura", "https://mainnet.infura.io"));
            //Local = new NodeModel(new EthereumNodeService("Local", "http://localhost:8545"), true, true);
            //Local = new NodeModel(new EthereumNodeService(new Web3("http://localhost:8546")));
        }

    }
}
