using CypherUtil;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace KeyStore
{
    public enum KeyType
    {
        EthNode = 0,
        MarketData = 1
    }

    public class KeyProperties
    {
        public string Value { get; private set; }
        public int Type { get; private set; }
        public string DisplayName { get; private set; }
        public int DisplayOrder { get; private set; }
        public int FastQueryInterval { get; private set; }
        public int SlowQueryInterval { get; private set; }

        public KeyProperties(string value, int type, string displayName, int displayOrder, int fastQueryInterval, int slowQueryInterval, int queryIntervalMultiplier)
        {
            Value = value;
            Type = type;
            DisplayName = displayName;
            DisplayOrder = displayOrder;
            FastQueryInterval = fastQueryInterval * queryIntervalMultiplier;
            SlowQueryInterval = slowQueryInterval * queryIntervalMultiplier;
        }
    }

    public class MSSQLServerStore
    {
        string connectionStr = null;
        public string CoinMarketCapApiKey { get; private set; }
        public string NomicsApiKey { get; private set; }
        public string InfuraMainnetKey { get; private set; }
        public string AlchemyMainnetKey { get; private set; }
        public string ChainstackEth1Node1Key { get; private set; }
        public string WhaleAlertKey { get; private set; }
        public string GetBlockKey { get; private set; }
        public string QuickNode { get; private set; }
        public string AnyBlockMainnetKey { get; private set; }
        public string ArchiveNodeKey { get; private set; }
        public string GethKey { get; private set; }
        public Dictionary<string, KeyProperties> KeyCollection { get; private set; }

        public bool GetApiKeys()
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                var command = new SqlCommand("select [Key], Value, KeyType, d.DisplayName, d.DisplayOrder, d.FastQueryInterval, d.SlowQueryInterval, d.QueryIntervalMultiplier from KeyStore k join DisplayProperties d on d.KeyId = k.Id", conn);

                command.Connection.Open();
                KeyCollection = new Dictionary<string, KeyProperties>();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var key = reader["Key"].ToString();
                    var value = reader["Value"].ToString();
                    var keyType = Convert.ToInt32(reader["KeyType"]);
                    var displayName = reader["DisplayName"].ToString();
                    var fastQueryInterval = Convert.ToInt32(reader["FastQueryInterval"]);
                    var slowQueryInterval = Convert.ToInt32(reader["SlowQueryInterval"]);

                    KeyCollection.Add(reader["Key"].ToString(), new KeyProperties(reader["Value"].ToString(), Convert.ToInt32(reader["KeyType"]), reader["DisplayName"].ToString()
                        , Convert.ToInt32(reader["DisplayOrder"]), Convert.ToInt32(reader["FastQueryInterval"]), Convert.ToInt32(reader["SlowQueryInterval"]), Convert.ToInt32(reader["QueryIntervalMultiplier"])));

                    switch (reader["Key"])
                    {
                        case "Geth":
                            GethKey = reader["Value"].ToString();
                            break;

                        case "Infura":
                            InfuraMainnetKey = reader["Value"].ToString();
                            break;

                        case "Alchemy":
                            AlchemyMainnetKey = reader["Value"].ToString();
                            break;

                        case "GetBlock":
                            GetBlockKey = reader["Value"].ToString();
                            break;

                        case "Chainstack-ETH-Node-1":
                            ChainstackEth1Node1Key = reader["Value"].ToString();
                            break;

                        case "QuickNode":
                            QuickNode = reader["Value"].ToString();
                            break;

                        case "Nomics":
                            NomicsApiKey = reader["Value"].ToString();
                            break;

                        case "CoinMarketCap":
                            CoinMarketCapApiKey = reader["Value"].ToString();
                            break;

                        case "WhaleAlert":
                            WhaleAlertKey = reader["Value"].ToString();
                            break;

                        case "AnyBlock":
                            AnyBlockMainnetKey = reader["Value"].ToString();
                            break;

                        case "ArchiveNode":
                            ArchiveNodeKey = reader["Value"].ToString();
                            break;

                        default:
                            Console.WriteLine($"Unused Key: {reader["Key"].ToString()}");
                            break;

                    }
                }
            }

            return KeyCollection != null && KeyCollection.Count > 0;
        }

        public void StoreApiKey(string key, string value)
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                var command = new SqlCommand($"insert KeyStore ([Key], Value) values('{key}','{value}')", conn);

                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public bool LogIn(string password1, string password2)
        {
            //var encrypted = Symmetric.Encrypt<AesManaged>(decrypted, password1, password2);
            var encrypted = "4Y390/+dcY9jvQUrfSwKdncyF/Ue0yOgfBH9/wPmfhFNJ7SJqV42Gl+Ykz4xlyR6JbYAaLPNbG2M8HJ1fCaF2XsRMthdI7UfazyngX5/Ax/TOGBRzWoMfhBWTK4VJF7Xb3mtLNu3ODU3//XNyJhmhZE/kpGgtIBUndyJUZT9dfpfA1HhoCwJVZsqu0kphtVgtw8UjXopg1URcybCG/wflg==";

            connectionStr = Symmetric.Decrypt<AesManaged>(encrypted, password1, password2);

            if (string.IsNullOrEmpty(connectionStr))
            {
                return false;
            }

            return true;
        }
    }
}
