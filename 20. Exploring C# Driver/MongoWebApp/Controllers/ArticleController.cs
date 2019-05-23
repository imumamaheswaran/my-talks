namespace MongoWebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Driver;

    [Route("api/[controller]")]
    public class ArticleController : Controller
    {
        public static string ConnectionString { get; } =
            "mongodb://uma:wYkyg9u2BYhQvuqOka8Z9ofd0gl1VJ6FwicO5xnUzmfOGpfGZOFL1whGbvgC9quZRo9tl9kENZaL7vvYgcG4Dw==@uma.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";

        private readonly MongoContext _context;
        public ArticleController()
        {
            _context = new MongoContext();
        }


        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var articles = new List<Article>();

            var projection = Builders<Article>.Projection.Exclude(f => f.Content);

            var findOptions = new FindOptions<Article, Article>()
            {
                Projection = projection
            };

            var filter = Builders<Article>.Filter.Where(f => f.Id == new Guid("a29502e5-91e3-49ca-bc64-549f8ea7a5d1"));

            var articleCursor = await _context.Articles.FindAsync(filter, findOptions);



            while (await articleCursor.MoveNextAsync())
            {
                articles.AddRange(articleCursor.Current);
            }

            return Ok(articles);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut]
        public void Put([FromBody] Article article)
        {
            article.Id = Guid.NewGuid();
            _context.Articles.InsertOne(article);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
