using RESTApi;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WhaleAlert
{
    public class WhaleAlertConnection : IConnection
    {
        //https://api.whale-alert.io/v1
        const string baseUrl = "https://api.whale-alert.io";
        HttpClient client = null;

        public WhaleAlertConnection(HttpClient client)
        {
            if (client == null)
            {
                throw new NullReferenceException("Http client required.");
            }

            this.client = client;
        }

        public async Task<string> ApiGet(string function, string apiKey)
        {
            var response = await Get(string.Format("{0}{1}", baseUrl, function), apiKey).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadAsStringAsync().ConfigureAwait(false)).Trim(new char[] { ' ', '"' });
            }

            return response.StatusCode.ToString();
        }

        private async Task<HttpResponseMessage> Get(string url, string apiKey)
        {
            //X-WA-API-KEY
            var key = apiKey.Trim().Split('=');

            if (key.Length > 1)
            {
                this.client.DefaultRequestHeaders.Add(key[0], key[1]);
                this.client.Timeout = TimeSpan.FromSeconds(30);

                return await client.GetAsync(url).ConfigureAwait(false);
            }

            return null;
        }
    }
}
