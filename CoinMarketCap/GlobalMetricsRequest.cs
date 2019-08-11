using RESTApi;
using System.Threading.Tasks;

namespace CoinMarketCap
{
    public static class GlobalMetricsRequest
    {
        public async static Task<string> GetGlobalMetrics(IConnection cmcConn, string apiKey)
        {
            return await cmcConn.ApiGet("/v1/global-metrics/quotes/latest", apiKey).ConfigureAwait(false);
        }
    }
}
