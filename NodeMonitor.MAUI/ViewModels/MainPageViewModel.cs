using CommunityToolkit.Mvvm.ComponentModel;
using KeyStore;
using NodeModels2;
using NodeServices;
using System.Collections.ObjectModel;

namespace NodeMonitor.MAUI
{
    public partial class MainPageViewModel : ObservableObject
    {
        MSSQLServerStore store = new MSSQLServerStore();

        [ObservableProperty]
        ObservableCollection<NodeModel> nodes;
        List<Tuple<string, string>> unloggedErrors = new List<Tuple<string, string>>();

        public MainPageViewModel()
        {
            Nodes = new ObservableCollection<NodeModel>();
        }

        public void Login(string password1, string password2)
        {
            store.LogIn(password1, password2);

            if (store.GetApiKeys())
            {
                var httpClient = new HttpClient();

                foreach (var key in store.KeyCollection)
                {
                    if (key.Value.Type == (int)KeyType.EthNode && !string.IsNullOrEmpty(key.Value.Value))
                    {
                        var executionClientSvc = new ExecutionClientService(key.Value.DisplayName, key.Value.ELEndpoint, httpClient);
                        var consensusClientSvc = new ConsensusClientService(key.Value.DisplayName, key.Value.CLEndpoint, httpClient);

                        executionClientSvc.Error += Svc_Error;
                        
                        var node = new NodeModel(executionClientSvc, consensusClientSvc, false, false, key.Value.FastQueryInterval, key.Value.SlowQueryInterval);

                        node.Error += Node_Error;
                        node.SlowQueryComplete += Node_SlowQueryComplete;
                        Nodes.Add(node);
                    }
                    else if(key.Value.Type == (int)KeyType.Eth1Node && !string.IsNullOrEmpty(key.Value.Value))
                    {
                        var executionClientSvc = new ExecutionClientService(key.Value.DisplayName, key.Value.ELEndpoint, httpClient);

                        executionClientSvc.Error += Svc_Error;

                        var node = new NodeModel(executionClientSvc, null, false, false, key.Value.FastQueryInterval, key.Value.SlowQueryInterval);

                        node.Error += Node_Error;
                        node.SlowQueryComplete += Node_SlowQueryComplete;
                        Nodes.Add(node);
                    }
                }
            }
        }

        private void Svc_Error<EventArgs>(object sender, EventArgs e)
        {

        }
        private void Node_Error<EventArgs>(object sender, EventArgs e)
        {

        }
        private void Node_SlowQueryComplete<EventArgs>(object sender, EventArgs e)
        {

        }
    }
}
