using System.Threading.Tasks;

namespace CoinMarketCap
{
    public static class GlobalMetricsRequest
    {
        public async static Task<string> GetGlobalMetrics(string apiKey)
        {
            return await CMCConnection.ApiGet("/v1/global-metrics/quotes/latest", apiKey).ConfigureAwait(false);
        }
    }
}
