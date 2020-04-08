using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;


namespace _01_CreatingMultiPartition
{
    public interface IInteraction
    {
        string type { get; }
    }
    public class PurchaseFoodOrBeverage : IInteraction
    {
        public string id { get; set; }
        public decimal unitPrice { get; set; }
        public decimal totalPrice { get; set; }
        public int quantity { get; set; }
        public string type { get; set; }
    }   
    public class ViewMap : IInteraction
    {
        public string id { get; set; }
        public int minutesViewed { get; set; }
        public string type { get; set; }
    }   
    public class Program
    {
        private static readonly string _endpointUri = "https://cosmoslab5842.documents.azure.com:443/";
        private static readonly string _primaryKey = "AMl2HHf6Me1HFNDiqRwL8bTMox5JHroZLhnz4kXW7MTVXRez1EI0LEhkoQHBD1fFJFlaUN96Ca2J3AllmL0pVQ==";
        public static async Task Main(string[] args)
        {
            using (CosmosClient client = new CosmosClient(_endpointUri, _primaryKey))
            {   /*
                DatabaseResponse databaseResponse = await client.CreateDatabaseIfNotExistsAsync("EntertainmentDatabase");
                Database targetDatabase = databaseResponse.Database;
                await Console.Out.WriteLineAsync($"Database Id:\t{targetDatabase.Id}");

                ContainerResponse response = await targetDatabase.CreateContainerIfNotExistsAsync("DefaultCollection", "/id");
                Container defaultContainer = response.Container;
                await Console.Out.WriteLineAsync($"Default Container Id:\t{defaultContainer.Id}");
                IndexingPolicy indexingPolicy = new IndexingPolicy
                {
                    IndexingMode = IndexingMode.Consistent,
                    Automatic = true,
                    IncludedPaths =
                    {
                        new IncludedPath
                        {
                            Path = "/*"
                        }
                    }
                };
                
                ContainerProperties containerProperties = new ContainerProperties("CustomCollection", "/type")
                {
                    IndexingPolicy = indexingPolicy,
                };
                var containerResponse = await targetDatabase.CreateContainerIfNotExistsAsync(containerProperties, 10000);
                var customContainer = containerResponse.Container;
                await Console.Out.WriteLineAsync($"Custom Container Id:\t{customContainer.Id}");
                */

                /* Task I: Populate Container with Data
                targetDatabase = client.GetDatabase("EntertainmentDatabase");
                customContainer = targetDatabase.GetContainer("CustomCollection");
                var foodInteractions = new Bogus.Faker<PurchaseFoodOrBeverage>()
                .RuleFor(i => i.id, (fake) => Guid.NewGuid().ToString())
                .RuleFor(i => i.type, (fake) => nameof(PurchaseFoodOrBeverage))
                .RuleFor(i => i.unitPrice, (fake) => Math.Round(fake.Random.Decimal(1.99m, 15.99m), 2))
                .RuleFor(i => i.quantity, (fake) => fake.Random.Number(1, 5))
                .RuleFor(i => i.totalPrice, (fake, user) => Math.Round(user.unitPrice * user.quantity, 2))
                .GenerateLazy(500);
                
                foreach(var interaction in foodInteractions)
                {
                    ItemResponse<PurchaseFoodOrBeverage> result = await customContainer.CreateItemAsync(interaction);
                    await Console.Out.WriteLineAsync($"Item Created\t{result.Resource.id}");
                }
                */
                var targetDatabase = client.GetDatabase("EntertainmentDatabase");
                var customContainer = targetDatabase.GetContainer("CustomCollection");
                var mapInteractions = new Bogus.Faker<ViewMap>()
                .RuleFor(i => i.id, (fake) => Guid.NewGuid().ToString())
                .RuleFor(i => i.type, (fake) => nameof(ViewMap))
                .RuleFor(i => i.minutesViewed, (fake) => fake.Random.Number(1, 45))
                .GenerateLazy(500);
                
                foreach(var interaction in mapInteractions)
                {
                    ItemResponse<ViewMap> result = await customContainer.CreateItemAsync(interaction);
                    await Console.Out.WriteLineAsync($"Document Created\t{result.Resource.id}");
                }
            }
        }
    }
}
