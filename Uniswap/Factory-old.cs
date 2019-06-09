using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace Uniswap
{
    public partial class Factory
    {
        public const string factoryABI = "[{\"name\": \"NewExchange\", \"inputs\": [{\"type\": \"address\", \"name\": \"token\", \"indexed\": true}, {\"type\": \"address\", \"name\": \"exchang\", \"indexed\": true}], \"anonymous\": false, \"type\": \"event\"}, {\"name\": \"initializeFactory\", \"outputs\": [], \"inputs\": [{\"type\": \"address\", \"name\": \"template\"}], \"constant\": false, \"payable\": false, \"type\": \"function\", \"gas\": 35725}, {\"name\": \"createExchange\", \"outputs\": [{\"type\": \"address\", \"name\": \"out\"}], \"inputs\": [{\"type\": \"address\", \"name\": \"token\"}], \"constant\": false, \"payable\": false, \"type\": \"function\", \"gas\": 187911}, {\"name\": \"getExchange\", \"outputs\": [{\"type\": \"address\", \"name\": \"out\"}], \"input\": [{\"typ\": \"address\", \"name\": \"token\"}], \"constant\": true, \"payable\": false, \"type\": \"function\", \"gas\": 715}, {\"name\": \"getToken\", \"outputs\": [{\"type\": \"address\", \"name\":\"out\"}], \"inputs\": [{\"type\": \"address\", \"name\": \"exchange\"}], \"constant\": true, \"payable\": false, \"type\": \"function\", \"gas\": 745}, {\"name\": \"getTokenWithId\", \"outputs\": [{\"type\": \"address\", \"name\": \"out\"}], \"inputs\": [{\"type\": \"uint256\", \"name\": \"token_id\"}], \"constant\": true, \"payable\": false, \"type\": \"function\", \"gas\": 736}, {\"name\": \"exchangeTemplate\", \"outputs\": [{\"type\": \"address\", \"name\": \"out\"}], \"inputs\": [], \"constant\": true, \"payable\": false, \"type\": \"function\", \"gas\": 633}, {\"name\": \"tokenCount\", \"outputs\": [{\"type\": \"uint256\", \"name\": \"out\"}], \"inputs\": [], \"constant\": true, \"payable\": false, \"type\": \"function\", \"gas\": 663}]";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("inputs")]
        public Input[] Inputs { get; set; }

        [JsonProperty("anonymous", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Anonymous { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("outputs", NullValueHandling = NullValueHandling.Ignore)]
        public Output[] Outputs { get; set; }

        [JsonProperty("constant", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Constant { get; set; }

        [JsonProperty("payable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Payable { get; set; }

        [JsonProperty("gas", NullValueHandling = NullValueHandling.Ignore)]
        public long? Gas { get; set; }
    }

    public partial class Input
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("indexed", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Indexed { get; set; }
    }

    public partial class Output
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Factory
    {
        public static Factory[] FromJson(string json) => JsonConvert.DeserializeObject<Factory[]>(json, Uniswap.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Factory[] self) => JsonConvert.SerializeObject(self, Uniswap.Converter.Settings);
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
