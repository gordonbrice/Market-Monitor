using System.Threading.Tasks;

namespace CoinMarketCap
{
    public static class GlobalMetricsRequest
    {
        public async static Task<string> GetGlobalMetrics()
        {
            return await CMCConnection.ApiGet("/v1/global-metrics/quotes/latest").ConfigureAwait(false);
        }
    }
}
