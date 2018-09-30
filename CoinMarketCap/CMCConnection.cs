using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoinMarketCap
{
    public static class CMCConnection
    {
        const string baseUrl = "https://pro-api.coinmarketcap.com";

        public async static Task<string> ApiGet(string function, string apiKey)
        {
            var response = await Get(string.Format("{0}{1}", baseUrl, function), apiKey).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadAsStringAsync().ConfigureAwait(false)).Trim(new char[] { ' ', '"' });
            }

            return response.StatusCode.ToString();
        }

        private async static Task<HttpResponseMessage> Get(string url, string apiKey)
        {
            var key = apiKey.Trim().Split('=');

            if(key.Length > 1)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add(key[0], key[1]);
                    client.Timeout = TimeSpan.FromSeconds(30);

                    return await client.GetAsync(url).ConfigureAwait(false);
                }
            }

            return null;
        }
    }
}
