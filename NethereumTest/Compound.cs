using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NethereumTest
{
    namespace Compound
    {
        using System;
        using System.Collections.Generic;

        using System.Globalization;
        using Newtonsoft.Json;
        using Newtonsoft.Json.Converters;

        public partial class MoneyMarket
        {
            [JsonProperty("type")]
            public MoneyMarketType Type { get; set; }

            [JsonProperty("stateMutability", NullValueHandling = NullValueHandling.Ignore)]
            public StateMutability? StateMutability { get; set; }

            [JsonProperty("payable", NullValueHandling = NullValueHandling.Ignore)]
            public bool? Payable { get; set; }

            [JsonProperty("outputs", NullValueHandling = NullValueHandling.Ignore)]
            public Output[] Outputs { get; set; }

            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("inputs", NullValueHandling = NullValueHandling.Ignore)]
            public Input[] Inputs { get; set; }

            [JsonProperty("constant", NullValueHandling = NullValueHandling.Ignore)]
            public bool? Constant { get; set; }

            [JsonProperty("anonymous", NullValueHandling = NullValueHandling.Ignore)]
            public bool? Anonymous { get; set; }
        }

        public partial class Input
        {
            [JsonProperty("type")]
            public InputType Type { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("indexed", NullValueHandling = NullValueHandling.Ignore)]
            public bool? Indexed { get; set; }
        }

        public partial class Output
        {
            [JsonProperty("type")]
            public InputType Type { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public enum InputType { Address, Bool, Int256, Uint256 };

        public enum StateMutability { Nonpayable, Payable, View };

        public enum MoneyMarketType { Constructor, Event, Fallback, Function };

        public partial class MoneyMarket
        {
            public static MoneyMarket[] FromJson(string json) => JsonConvert.DeserializeObject<MoneyMarket[]>(json, Compound.Converter.Settings);
        }

        public static class Serialize
        {
            public static string ToJson(this MoneyMarket[] self) => JsonConvert.SerializeObject(self, Compound.Converter.Settings);
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
            {
                InputTypeConverter.Singleton,
                StateMutabilityConverter.Singleton,
                MoneyMarketTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        internal class InputTypeConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(InputType) || t == typeof(InputType?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "address":
                        return InputType.Address;
                    case "bool":
                        return InputType.Bool;
                    case "int256":
                        return InputType.Int256;
                    case "uint256":
                        return InputType.Uint256;
                }
                throw new Exception("Cannot unmarshal type InputType");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (InputType)untypedValue;
                switch (value)
                {
                    case InputType.Address:
                        serializer.Serialize(writer, "address");
                        return;
                    case InputType.Bool:
                        serializer.Serialize(writer, "bool");
                        return;
                    case InputType.Int256:
                        serializer.Serialize(writer, "int256");
                        return;
                    case InputType.Uint256:
                        serializer.Serialize(writer, "uint256");
                        return;
                }
                throw new Exception("Cannot marshal type InputType");
            }

            public static readonly InputTypeConverter Singleton = new InputTypeConverter();
        }

        internal class StateMutabilityConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(StateMutability) || t == typeof(StateMutability?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "nonpayable":
                        return StateMutability.Nonpayable;
                    case "payable":
                        return StateMutability.Payable;
                    case "view":
                        return StateMutability.View;
                }
                throw new Exception("Cannot unmarshal type StateMutability");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (StateMutability)untypedValue;
                switch (value)
                {
                    case StateMutability.Nonpayable:
                        serializer.Serialize(writer, "nonpayable");
                        return;
                    case StateMutability.Payable:
                        serializer.Serialize(writer, "payable");
                        return;
                    case StateMutability.View:
                        serializer.Serialize(writer, "view");
                        return;
                }
                throw new Exception("Cannot marshal type StateMutability");
            }

            public static readonly StateMutabilityConverter Singleton = new StateMutabilityConverter();
        }

        internal class MoneyMarketTypeConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(MoneyMarketType) || t == typeof(MoneyMarketType?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "constructor":
                        return MoneyMarketType.Constructor;
                    case "event":
                        return MoneyMarketType.Event;
                    case "fallback":
                        return MoneyMarketType.Fallback;
                    case "function":
                        return MoneyMarketType.Function;
                }
                throw new Exception("Cannot unmarshal type MoneyMarketType");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (MoneyMarketType)untypedValue;
                switch (value)
                {
                    case MoneyMarketType.Constructor:
                        serializer.Serialize(writer, "constructor");
                        return;
                    case MoneyMarketType.Event:
                        serializer.Serialize(writer, "event");
                        return;
                    case MoneyMarketType.Fallback:
                        serializer.Serialize(writer, "fallback");
                        return;
                    case MoneyMarketType.Function:
                        serializer.Serialize(writer, "function");
                        return;
                }
                throw new Exception("Cannot marshal type MoneyMarketType");
            }

            public static readonly MoneyMarketTypeConverter Singleton = new MoneyMarketTypeConverter();
        }
    }
}
