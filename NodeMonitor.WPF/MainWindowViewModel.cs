using KeyStore;
using MVVMSupport;
using NodeModels;
using NodeServices;
using System.Collections.ObjectModel;
using System.Net.Http;
using Utilities;

namespace NodeMonitor.WPF
{
    public class MainWindowViewModel : ViewModelBase
    {
        MSSQLServerStore store = new MSSQLServerStore();
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
            Nodes = new ObservableCollection<NodeModel>();

            var passwordDlg = new PasswordDialog();

            if (passwordDlg.ShowDialog() == true)
            {
                var password1 = passwordDlg.Password1;
                var password2 = passwordDlg.Password2;

                store.LogIn(password1, password2);
                
                if(store.GetApiKeys())
                {
                    var httpClient = new HttpClient();

                    foreach(var key in store.KeyCollection)
                    {
                        if(key.Value.Type == (int)KeyType.EthNode && !string.IsNullOrEmpty(key.Value.Value))
                        {
                            var node = new NodeModel(new EthereumNodeService(key.Value.DisplayName, key.Value.Value, httpClient), false, false, key.Value.FastQueryInterval, key.Value.SlowQueryInterval);

                            node.Error += Node_Error;
                            Nodes.Add(node);
                        }
                    }
                }

            }
        }

        private void Node_Error(object sender, NodeErrorEventArgs e)
        {
            //this.store.Log(e.Name, e.Message).Wait();

            
        }
    }
}
