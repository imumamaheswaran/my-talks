
namespace MongoWebApp
{
    using System;
    using System.Security.Authentication;
    using MongoDB.Driver;

    public class MongoContext
    {
        private string ConnectionString { get; } =
            "mongodb://uma:wYkyg9u2BYhQvuqOka8Z9ofd0gl1VJ6FwicO5xnUzmfOGpfGZOFL1whGbvgC9quZRo9tl9kENZaL7vvYgcG4Dw==@uma.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";

        private string Database { get; } = "uma";

        private readonly IMongoDatabase _mongoDatabase;

        public MongoContext()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException(nameof(ConnectionString));
            if (string.IsNullOrEmpty(Database))
                throw new ArgumentNullException(nameof(Database));

            var settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
            
                settings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = SslProtocols.Tls12
                };
            

            var client = new MongoClient(settings);
            _mongoDatabase = client.GetDatabase(Database);
        }

        public IMongoCollection<Article> Articles => _mongoDatabase.GetCollection<Article>("articles");
    }
}
