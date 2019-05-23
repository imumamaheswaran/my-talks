using System;
using static System.Console;
namespace HelloMongo
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    class Program
    {
        /**
         * Connecting to MongoDB with the new v2 API
         * 
         * Generics
         * Expressions
         * 
         * Talk about ToListAsync
         * Filtering documents
         *  With BsonDocument()
         *  With Strongly typed Linq syntax
         * Definitions and Builders
         *  Render()
         * **/
        static void Main(string[] args)
        {
            var client =
                new MongoClient(
                    "mongodb://uma:wYkyg9u2BYhQvuqOka8Z9ofd0gl1VJ6FwicO5xnUzmfOGpfGZOFL1whGbvgC9quZRo9tl9kENZaL7vvYgcG4Dw==@uma.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            // Pool of connections
            // With only one instance of MongoClient you can actually handle multiple threads


            var database = client.GetDatabase("uma");

            var collections = database.GetCollection<BsonDocument>("articles");

            var count = collections.Count(FilterDefinition<BsonDocument>.Empty);

            var buildInfoCommand = new BsonDocument("buildinfo", 1);
            var buildInfo = database.RunCommandAsync<BsonDocument>(buildInfoCommand);

            WriteLine(buildInfo.ToJson());
            var document = new BsonDocument
            {
                {"hidden", false}
            };
            var documents = collections.Find(new BsonDocument
            {
                {"hidden", true}
            }).ToList();
            WriteLine(documents.Count);
            WriteLine("Hello World!");
            Read();
        }
    }
}
