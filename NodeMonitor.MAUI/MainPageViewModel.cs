using CommunityToolkit.Mvvm.ComponentModel;
using KeyStore;
using NodeModels2;
using System.Collections.ObjectModel;

namespace NodeMonitor.MAUI
{
    internal partial class MainPageViewModel : ObservableObject
    {
        MSSQLServerStore store = new MSSQLServerStore();

        [ObservableProperty]
        ObservableCollection<NodeModel> nodes;
        List<Tuple<string, string>> unloggedErrors = new List<Tuple<string, string>>();

        public MainPageViewModel()
        {
            Nodes = new ObservableCollection<NodeModel>();

            //var passwordDlg = new PasswordDialog();

            //if (passwordDlg.ShowDialog() == true)
            //{
            //    var password1 = passwordDlg.Password1;
            //    var password2 = passwordDlg.Password2;

            //    store.LogIn(password1, password2);

            //    if (store.GetApiKeys())
            //    {
            //        var httpClient = new HttpClient();

            //        foreach (var key in store.KeyCollection)
            //        {
            //            if (key.Value.Type == (int)KeyType.EthNode && !string.IsNullOrEmpty(key.Value.Value))
            //            {
            //                var svc = new EthereumNodeService(key.Value.DisplayName, key.Value.Value, httpClient);

            //                svc.Error += Svc_Error;

            //                var node = new NodeModel(svc, false, false, key.Value.FastQueryInterval, key.Value.SlowQueryInterval);

            //                node.Error += Node_Error;
            //                node.SlowQueryComplete += Node_SlowQueryComplete;
            //                Nodes.Add(node);
            //            }
            //        }
            //    }
            //}
        }
    }
}
