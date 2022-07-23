using KeyStore;
using MVVMSupport;
using NodeModels;
using NodeServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Utilities;

namespace NodeMonitor.WPF
{
    public class MainWindowViewModel : ViewModelBase
    {
        MSSQLServerStore store = new MSSQLServerStore();
        ObservableCollection<NodeModel> nodes;
        List<Tuple<string, string>> unloggedErrors = new List<Tuple<string, string>>();

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
                            var svc = new ExecutionClientService(key.Value.DisplayName, key.Value.Value, key.Value.ELPort, httpClient);

                            svc.Error += Svc_Error;

                            var node = new NodeModel(svc, false, false, key.Value.FastQueryInterval, key.Value.SlowQueryInterval);

                            node.Error += Node_Error;
                            node.SlowQueryComplete += Node_SlowQueryComplete;
                            Nodes.Add(node);
                        }
                    }
                }

            }
        }

        private void Svc_Error(object sender, EthNodeServiceErrorEventArgs e)
        {
            var ex = new NodeErrorEventArgs
            {
                Name = e.Name,
                Message = e.Message
            };

            Node_Error(sender, ex);
        }

        private void Node_SlowQueryComplete(object sender, SlowQuertEventArgs e)
        {
            this.store.RollupLogs().Wait();
        }

        private void Node_Error(object sender, NodeErrorEventArgs e)
        {
            try
            {
                this.store.Log(e.Name, e.Message).Wait();

                if(unloggedErrors.Count > 0)
                {
                    if(unloggedErrors[0].Item1 != null && unloggedErrors[0].Item2 != null)
                    {
                        this.store.Log(unloggedErrors[0].Item1, unloggedErrors[0].Item2).Wait();
                    }

                    unloggedErrors.RemoveAt(0);
                }
            }
            catch(Exception x)
            {
                unloggedErrors.Add(new Tuple<string,string>(e.Name, e.Message));
                unloggedErrors.Add(new Tuple<string, string>("DataStore", x.Message));
            }
        }
    }
}
