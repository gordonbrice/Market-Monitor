using RESTApi;
using System.Threading.Tasks;

namespace Nomics
{
    public static class MarketHistoryRequest
    {
        public static async Task<string> GetMarketCapHistory(IConnection nomicsConn, string apiKey)
        {
            return await nomicsConn.ApiGet("v1/market-cap/history", apiKey);
        }
        
        public static async Task<string> GetGlobalVolumeHistory(IConnection nomicsConn, string apiKey)
        {
            return await nomicsConn.ApiGet("v1/volume/history", apiKey);
        }
    }
}
