using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Nomics
{
    public class NomicsConnection
    {
        const string baseUrl = "https://api.nomics.com/";

        public async static Task<string> ApiGet(string function, string key)
        {
            var span = TimeSpan.FromDays(1);
            var now = DateTime.Now;
            var param = HttpUtility.UrlEncode(string.Format("{0}-{1}-{2}T00:00:00Z", now.Year, now.Month, now.Day - 1));

            //var request = string.Format("{0}{1}?{2}\"&\"start=", baseUrl, function, key) + HttpUtility.UrlEncode(string.Format("{0}-{1}-{2}T00:00:00Z", now.Year, now.Month, now.Day - 1));
            var request = string.Format("{0}{1}?{2}&start={3}", baseUrl, function, key, param);
            var response = await Get(request).ConfigureAwait(false);

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
