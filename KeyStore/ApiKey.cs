using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyStore
{
    public class ApiKey
    {
        [BsonId]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
