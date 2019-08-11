using RESTApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoinMarketCap
{
    public class CryptoCurrencysRequest
    {
        public async static Task<string> GetQuotes(IConnection cmcConn, string apiKey, string commaDelimitedCryptos)
        {
            return await cmcConn.ApiGet($"/v1/cryptocurrency/quotes/latest?symbol={commaDelimitedCryptos}", apiKey).ConfigureAwait(false);
        }
    }
}
