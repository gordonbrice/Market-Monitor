using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace Nomics
{
    public partial class MarketCapHistory
    {
        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("market_cap")]
        public string MarketCap { get; set; }
    }

    public partial class MarketCapHistory
    {
        public static MarketCapHistory[] FromJson(string json) => JsonConvert.DeserializeObject<MarketCapHistory[]>(json, Nomics.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this MarketCapHistory[] self) => JsonConvert.SerializeObject(self, Nomics.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
