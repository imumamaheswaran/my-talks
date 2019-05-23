namespace MongoWebApp
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    public class Article
    {
        [BsonId]
        public Guid Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
    }
}
