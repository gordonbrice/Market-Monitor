using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoinMarketCap
{
    public class CryptoCurrencysRequest
    {
        public async static Task<string> GetQuotes(string apiKey, string commaDelimitedCryptos)
        {
            return await CMCConnection.ApiGet($"/v1/cryptocurrency/quotes/latest?symbol={commaDelimitedCryptos}", apiKey).ConfigureAwait(false);
        }
    }
}
