using System.Threading.Tasks;

namespace Nomics
{
    public static class MarketCapHistory
    {
        public static async Task<string> GetMarketCapHistory(string apiKey)
        {
            return await NomicsConnection.ApiGet("v1/market-cap/history", apiKey);
        }
    }
}
