﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using WhaleAlert;
//
//    var status = Status.FromJson(jsonString);

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace WhaleAlert
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Status
    {
        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("blockchain_count")]
        public long BlockchainCount { get; set; }

        [JsonProperty("blockchains")]
        public Blockchain[] Blockchains { get; set; }
    }

    public partial class Blockchain
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbols")]
        public string[] Symbols { get; set; }

        [JsonProperty("status")]
        public StatusEnum Status { get; set; }
    }

    public enum StatusEnum { Connected };

    public partial class Status
    {
        public static Status FromJson(string json) => JsonConvert.DeserializeObject<Status>(json, WhaleAlert.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Status self) => JsonConvert.SerializeObject(self, WhaleAlert.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                StatusEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class StatusEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(StatusEnum) || t == typeof(StatusEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "connected")
            {
                return StatusEnum.Connected;
            }
            throw new Exception("Cannot unmarshal type StatusEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (StatusEnum)untypedValue;
            if (value == StatusEnum.Connected)
            {
                serializer.Serialize(writer, "connected");
                return;
            }
            throw new Exception("Cannot marshal type StatusEnum");
        }

        public static readonly StatusEnumConverter Singleton = new StatusEnumConverter();
    }
}
