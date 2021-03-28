using CypherUtil;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KeyStore
{
    public class MongoAtlasStore
    {
        string connectionStr = null;
        public string CoinMarketCapApiKey { get; private set; }
        public string NomicsApiKey { get; private set; }
        public string InfuraMainnetKey { get; private set; }
        public string AlchemyMainnetKey { get; private set; }
        public string ChainstackEth1Node1Key{get; private set; }
        public string WhaleAlertKey { get; private set; }
        public string GetBlockKey { get; private set; }

        public void GetApiKeys()
        {
            var collection = new MongoClient(this.connectionStr).GetDatabase("key-store").GetCollection<ApiKey>("api-keys");
            var keys = collection.Find(k => k.Name == "Infura" || k.Name == "Alchemy" || k.Name == "Chainstack-ETH-Node-1" || k.Name == "GetBlock").ToList();

            foreach (var apiKey in keys)
            {
                switch (apiKey.Name)
                {
                    case "Infura":
                        InfuraMainnetKey = apiKey.Value;
                        break;

                    case "Alchemy":
                        AlchemyMainnetKey = apiKey.Value;
                        break;

                    case "GetBlock":
                        GetBlockKey = apiKey.Value;
                        break;

                    case "Chainstack-ETH-Node-1":
                        ChainstackEth1Node1Key = apiKey.Value;
                        break;

                    case "Nomics":
                        NomicsApiKey = apiKey.Value;
                        break;

                    case "CoinMarketCap":
                        CoinMarketCapApiKey = apiKey.Value;
                        break;

                    case "WhaleAlert":
                        WhaleAlertKey = apiKey.Value;
                        break;

                    default:
                        Console.WriteLine($"Unused Key: {apiKey.Name}");
                        break;

                }
            }
        }
        public void StoreApiKey(string name, string key)
        {
            var collection = new MongoClient(connectionStr).GetDatabase("key-store").GetCollection<ApiKey>("api-keys");
            var count = collection.CountDocuments(new BsonDocument());
            var apiKey = new ApiKey
            {
                Id = count + 1,
                Name = name,
                Value = key
            };

            collection.InsertOne(apiKey);
            
        }
        public bool LogIn(string password1, string password2)
        {
            //var encrypted = Symmetric.Encrypt<AesManaged>(decrypted, password1, password2);
            var encrypted = "MpellEPjKmPk0tzWRY6hSu9y+rnlGzB1jAdiQ+g3oksQ3tOtp5/fu90/YmRsOtFXIDhkHCEobDBDytKnGgumKkxE2Lr2Fv8PgCUuswKK9tp52rdH13EuNvsIZNkYjdchtjnzC3oNsZesneWq4zgO9hzOkSmqku4nNM3sHbhHq8TJmV1P1G8TE2TH/Us49vEcqRtRaOF0oU2vzCU32I4nBilKY2oVEvd9/6xTXfpdF88+X+Vi2D0E0wEf/oe3NAAnwedV2KF2nhgweWWtIENL/Q==";

            connectionStr = Symmetric.Decrypt<AesManaged>(encrypted, password1, password2);

            if(string.IsNullOrEmpty(connectionStr))
            {
                return false;
            }

            return true;
        }
    }
}
