using System.Threading.Tasks;

namespace Nomics
{
    public static class MarketHistoryRequest
    {
        public static async Task<string> GetMarketCapHistory(string apiKey)
        {
            return await NomicsConnection.ApiGet("v1/market-cap/history", apiKey);
        }
        
        public static async Task<string> GetGlobalVolumeHistory(string apiKey)
        {
            return await NomicsConnection.ApiGet("v1/volume/history", apiKey);
        }
    }
}
