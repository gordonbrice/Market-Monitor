using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nomics
{
    public class NomicsConnection
    {
        const string baseUrl = "https://api.nomics.com";

        public async static Task<string> ApiGet(string function, string key)
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
                client.Timeout = TimeSpan.FromSeconds(30);

                return await client.GetAsync(url).ConfigureAwait(false);
            }
        }
    }
}
