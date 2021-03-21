using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GridBlazor;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using KeyStore;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using WhaleAlert;

namespace AlertService
{
    public class BlockchainAlertService
    {
        WhaleAlertModel _whaleAlert;
        HttpClient _httpClient;
        MongoAtlasStore _keyStore;

        public string Status { get; set; }
        public long Count { get; set; }
        public BlockchainAlertService(HttpClient httpClient, MongoAtlasStore keyStore)
        {
            _httpClient = httpClient;
            _keyStore = keyStore;
        }
        public ItemsDTO<CryptoAsset> GetCryptoAssetGridRows(Action<IGridColumnCollection<CryptoAsset>> columns,
                QueryDictionary<StringValues> query)
        {
            var getAllStatusAwaiter = GetAllStatus().GetAwaiter();

            getAllStatusAwaiter.OnCompleted(() =>
            {
            });

            var server = new GridServer<CryptoAsset>(getAllStatusAwaiter.GetResult(), new QueryCollection(query),
                true, "cryptoAssetsGrid", columns, 10);

            // return items to displays
            return server.ItemsToDisplay;
        }

        private async Task<IList<CryptoAsset>>  GetAllStatus()
        {
            _whaleAlert = new WhaleAlertModel(_httpClient, _keyStore.WhaleAlertKey);
            await _whaleAlert.GetStatus();
            Status = _whaleAlert.Status.Result;
            Count = _whaleAlert.Status.BlockchainCount;

            var assets = new List<CryptoAsset>();

            if (_whaleAlert.Status.BlockchainCount > 0)
            {
                foreach (var blockchain in _whaleAlert.Status.Blockchains)
                {
                    foreach (var symbol in blockchain.Symbols)
                    {
                        assets.Add(new CryptoAsset(blockchain.Name, symbol, blockchain.Status.ToString()));
                    }
                }
            }

            return assets;
        }
    }
    public class CryptoAsset
    {
        public CryptoAsset(string blockchainName, string symbol, string status)
        {
            BlockChainName = blockchainName;
            Symbol = symbol;
            Status = status;
        }

        public string BlockChainName { get; private set; }
        public string Symbol { get; private set; }
        public string Status { get; private set; }
    }
}
