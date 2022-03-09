using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using RMotownFestival.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RMotownFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : Controller
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _websiteArticlesContainer;

        public ArticlesController(IConfiguration configuration)
        {
            _cosmosClient = new CosmosClient(configuration.GetConnectionString("CosmosConnection"));
            var _cosmosDatabase = _cosmosClient.CreateDatabaseIfNotExistsAsync("RMotownArticles");
            var test = _cosmosDatabase.Result.Database;
            test.CreateContainerIfNotExistsAsync(new ContainerProperties()
            {
                Id = "WebsiteArticles",
                PartitionKeyPath = "/tag"
            });
            _websiteArticlesContainer = _cosmosClient.GetContainer("RMotownArticles", "WebsiteArticles");
        }

        public IActionResult Index()
        {
            return Ok("werkt");
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromQuery] bool random, [FromBody] Article article)
        {
            if (random)
            {
                article = new Article()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "this is a published thing",
                    Tag = "supercool music",
                    Message = " be careful of covid",
                    ImagePath = "",
                    Status = Status.Published.ToString(),
                    Date = DateTime.Now
                };
            }
            await _websiteArticlesContainer.CreateItemAsync(article);
            return Ok();
        }

        [HttpGet("published")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Artist>))]
        public async Task<ActionResult> GetAsync()
        {
            var result = new List<Article>();

            var queryDefinition = _websiteArticlesContainer.GetItemLinqQueryable<Article>()
                .Where(p => p.Status == nameof(Status.Published))
                .OrderBy(p => p.Date);

            var iterator = queryDefinition.ToFeedIterator();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                result = response.ToList();
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArticle([FromBody] Article article)
        {
            var queryDefinition = _websiteArticlesContainer.GetItemLinqQueryable<Article>()
               .Where(p => p.Id == article.Id);

            var iterator = queryDefinition.ToFeedIterator();

            var articleFromDb = await iterator.ReadNextAsync();

            Article articleToDb = articleFromDb.Resource.FirstOrDefault();
            articleToDb.Title = article.Title;
            articleToDb.Message = article.Message;
            articleToDb.Date = article.Date;
            articleToDb.ImagePath = article.ImagePath;
            articleToDb.Status = article.Status;

            await _websiteArticlesContainer.ReplaceItemAsync(articleToDb, articleToDb.Id, new PartitionKey(articleToDb.Tag));

            
            return Ok("Updated successfully");
        }
    }
}