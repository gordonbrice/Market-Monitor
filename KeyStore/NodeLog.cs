using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeyStore
{
    public class NodeLog
    {
        [BsonId]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
