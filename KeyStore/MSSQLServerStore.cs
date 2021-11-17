using CypherUtil;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

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
        DateTime rollupDoneDate = DateTime.MinValue;
        public Dictionary<string, KeyProperties> KeyCollection { get; private set; }

        public bool GetApiKeys()
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                var command = new SqlCommand("select [Key], Value, KeyType, Disabled, d.DisplayName, d.DisplayOrder, d.FastQueryInterval, d.SlowQueryInterval, d.QueryIntervalMultiplier from KeyStore k join DisplayProperties d on d.KeyId = k.Id where Disabled = 0 order by d.DisplayOrder", conn);
                
                command.Connection.Open();
                KeyCollection = new Dictionary<string, KeyProperties>();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if(Convert.ToInt16(reader["Disabled"]) == 0)
                    {
                        KeyCollection.Add(reader["Key"].ToString(), new KeyProperties(reader["Value"].ToString(), Convert.ToInt32(reader["KeyType"]), reader["DisplayName"].ToString()
                            , Convert.ToInt32(reader["DisplayOrder"]), Convert.ToInt32(reader["FastQueryInterval"]), Convert.ToInt32(reader["SlowQueryInterval"]), Convert.ToInt32(reader["QueryIntervalMultiplier"])));
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

        public async Task Log(string name, string message)
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                var command = new SqlCommand($"insert Log (Name,Message,TimeStamp) values('{name}','{message}',GETDATE())", conn);

                command.Connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task RollupLogs()
        {
            if(rollupDoneDate == DateTime.MinValue || rollupDoneDate.DayOfYear < DateTime.Now.DayOfYear || rollupDoneDate.Year < DateTime.Now.Year)
            {
                using (var conn = new SqlConnection(connectionStr))
                {

                    var command = new SqlCommand("exec RollupLogs", conn);

                    command.Connection.Open();

                    var reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        if (reader["RollupDate"] == DBNull.Value)
                        {
                            rollupDoneDate = DateTime.Now;
                        }
                    }
                }
            }
        }
    }
}
