using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoinMarketCap
{
    public static class CMCConnection
    {
        const string baseUrl = "https://pro-api.coinmarketcap.com";
        //const string apiKey = "CMC_PRO_API_KEY=24a1792e-e16d-4a11-ba29-2cd79ed459bb";

        public async static Task<string> ApiGet(string function)
        {
            var response = await Get(string.Format("{0}{1}", baseUrl, function)).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadAsStringAsync().ConfigureAwait(false)).Trim(new char[] { ' ', '"' });
            }

            return response.StatusCode.ToString();
        }

        private async static Task<HttpResponseMessage> Get(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", "24a1792e-e16d-4a11-ba29-2cd79ed459bb");
                client.Timeout = TimeSpan.FromSeconds(30);

                return await client.GetAsync(url).ConfigureAwait(false);
            }
        }
    }
}
