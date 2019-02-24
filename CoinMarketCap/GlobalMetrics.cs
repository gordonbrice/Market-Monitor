using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace CoinMarketCap
{
    public partial class GlobalMetrics
    {
        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("active_cryptocurrencies")]
        public long ActiveCryptocurrencies { get; set; }

        [JsonProperty("active_market_pairs")]
        public long ActiveMarketPairs { get; set; }

        [JsonProperty("active_exchanges")]
        public long ActiveExchanges { get; set; }

        [JsonProperty("eth_dominance")]
        public double EthDominance { get; set; }

        [JsonProperty("btc_dominance")]
        public double BtcDominance { get; set; }

        [JsonProperty("quote")]
        public Quote Quote { get; set; }

        [JsonProperty("last_updated")]
        public DateTimeOffset LastUpdated { get; set; }
    }

    public partial class Quote
    {
        [JsonProperty("USD")]
        public Usd Usd { get; set; }
    }

    public partial class Usd
    {
        [JsonProperty("total_market_cap")]
        public double TotalMarketCap { get; set; }

        [JsonProperty("total_volume_24h")]
        public double TotalVolume24H { get; set; }

        [JsonProperty("last_updated")]
        public DateTimeOffset LastUpdated { get; set; }
    }

    public partial class Status
    {
        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("error_code")]
        public long ErrorCode { get; set; }

        [JsonProperty("error_message")]
        public object ErrorMessage { get; set; }

        [JsonProperty("elapsed")]
        public long Elapsed { get; set; }

        [JsonProperty("credit_count")]
        public long CreditCount { get; set; }
    }

    public partial class GlobalMetrics
    {
        public static GlobalMetrics FromJson(string json) => JsonConvert.DeserializeObject<GlobalMetrics>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GlobalMetrics self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
        };
    }
}
