using CommunityToolkit.Mvvm.ComponentModel;
using KeyStore;
using NodeModels2;
using NodeServices;
using System.Collections.ObjectModel;
using System.Net.WebSockets;
using System.Text;

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

        private async Task Receive(ClientWebSocket ws)
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);

            do
            {
                WebSocketReceiveResult result;

                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    ms.Seek(0, SeekOrigin.Begin);

                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        var msg = await reader.ReadToEndAsync();

                        Console.WriteLine(msg);
                    }
                }
            } while (true);

        }

        public async void Login(string password1, string password2)
        {
            store.LogIn(password1, password2);

            if (store.GetApiKeys())
            {
                var httpClient = new HttpClient();
                var url = new Uri("wss://node-mux.azurewebsites.net");

                using (var ws = new ClientWebSocket())
                {
                    await ws.ConnectAsync(url, CancellationToken.None);

                    var buf = new byte[1024];

                    await ws.SendAsync(Encoding.UTF8.GetBytes("Infura"), WebSocketMessageType.Text, true, CancellationToken.None);

                    var result = await ws.ReceiveAsync(buf, CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                        Console.WriteLine(result.CloseStatusDescription);
                    }
                    else
                    {
                        var msg = Encoding.UTF8.GetString(buf, 0, result.Count);

                        Console.WriteLine(msg);
                        await Receive(ws);
                    }
                }

                foreach (var key in store.KeyCollection)
                {
                    if (key.Value.Type == (int)KeyType.EthNode || (!string.IsNullOrEmpty(key.Value.CLEndpoint) && !string.IsNullOrEmpty(key.Value.ELEndpoint)))
                    {
                        var executionClientSvc = new ExecutionClientService(key.Value.DisplayName, key.Value.ELEndpoint, httpClient);
                        var consensusClientSvc = new ConsensusClientService(key.Value.DisplayName, key.Value.CLEndpoint, httpClient, key.Value.CLEndpointAuth);

                        executionClientSvc.Error += Svc_Error;

                        var node = new NodeModel(executionClientSvc, consensusClientSvc, false, false, key.Value.FastQueryInterval, key.Value.SlowQueryInterval);

                        node.Error += Node_Error;
                        node.SlowQueryComplete += Node_SlowQueryComplete;
                        Nodes.Add(node);
                    }
                    else if (key.Value.Type == (int)KeyType.Eth1Node && !string.IsNullOrEmpty(key.Value.Value))
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
