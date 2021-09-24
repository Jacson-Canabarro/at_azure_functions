using Infra.Model;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infra.Repository
{
    public class TodoRepository
    {
        private string ConnectionString = "AccountEndpoint=https://infnet-cosmos-db.documents.azure.com:443/;AccountKey=bNFZmbuVjZP0y8IcoSxN7brf8SZl6W6YZzXICeCeDDy4H0APuTQ6iMTMY3Eu7mFFUjmOIr5jZmtBxzEwdK3Vgg==;";
        private string Container = "todo-container";
        private string Database = "todo-db";


        private CosmosClient CosmosClient { get; set; }

        public TodoRepository()
        {
            this.CosmosClient = new CosmosClient(this.ConnectionString);
        }


        public List<TodoItem> GetAll()
        {
            var container = this.CosmosClient.GetContainer(Database, Container);

            QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c");

            var result = new List<TodoItem>();

            var queryResult = container.GetItemQueryIterator<TodoItem>(queryDefinition);

            while (queryResult.HasMoreResults)
            {
                FeedResponse<TodoItem> currentResultSet = queryResult.ReadNextAsync().Result;
                result.AddRange(currentResultSet.Resource);
            }

            return result;

        }

        public TodoItem GetById(string id)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);

            QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c where c.id = '{id}'");

            var queryResult = container.GetItemQueryIterator<TodoItem>(queryDefinition);

            TodoItem item = null;

            while (queryResult.HasMoreResults)
            {
                FeedResponse<TodoItem> currentResultSet = queryResult.ReadNextAsync().Result;
                item = currentResultSet.Resource.FirstOrDefault();
            }

            return item;
        }

        public async Task Save(TodoItem item)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);
            await container.CreateItemAsync<TodoItem>(item, new PartitionKey(item.PartitionKey));
        }

        public async Task Update(TodoItem item)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);
            await container.ReplaceItemAsync<TodoItem>(item, item.Id.ToString(), new PartitionKey(item.PartitionKey));
        }

        public async Task Delete(TodoItem item)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);
            await container.DeleteItemAsync<TodoItem>(item.Id.ToString(), new PartitionKey(item.PartitionKey));
        }


    }
}
