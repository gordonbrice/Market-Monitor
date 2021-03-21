using KeyStore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace MongoDBTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var client = new MongoClient("mongodb+srv://studient:passthetest@mflix.rqenq.mongodb.net/sample_mflix?retryWrites=true&w=majority");
            //var db = client.GetDatabase("sample_mflix");
            //var collection = db.GetCollection<BsonDocument>("movies");
            //var result = collection.Find("{title: 'The Princess Bride'}").FirstOrDefault();

            //Console.Write(result);

            //var docs = collection.Find(new BsonDocument())
            //               .SortByDescending(m => m["runtime"])
            //               .Limit(10)
            //               .ToList();

            //foreach (var d in docs)
            //{
            //    Console.WriteLine(d.GetValue("title"));
            //}

            Console.WriteLine("Password 1:");

            var password1 = Console.ReadLine();

            Console.WriteLine("Password 2:");

            var password2 = Console.ReadLine();

            if(string.IsNullOrEmpty(password1) || string.IsNullOrEmpty(password2))
            {
                Console.WriteLine("Invalid password.");
            }
            else
            {
                var store = new MongoAtlasStore();

                if(store.LogIn(password1, password2))
                {
                    Console.WriteLine("Key name:");

                    var name = Console.ReadLine();

                    Console.WriteLine("Key value:");

                    var key = Console.ReadLine();

                    if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(key))
                    {
                        store.StoreApiKey(name, key);
                        Console.WriteLine("Key inserted sucessfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid key.");
                    }
                }
                else
                {
                    Console.WriteLine("Login failed.");
                }
            }
        }
    }
}
