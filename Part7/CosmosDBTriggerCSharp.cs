using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Vainolo.AzureFunctionsTutorial.Part7
{
    public static class CosmosDBTriggerCSharp
    {
        [FunctionName("CosmosDBTriggerCSharp")]
        public static void Run(
            [CosmosDBTrigger(
                databaseName: "mydatabase",
                collectionName: "mycollection",
                ConnectionStringSetting = "MyCosmosDB")]IReadOnlyList<Document> input, 
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document text " + input[0].GetPropertyValue<string>("text"));
            }
        }
    }
}
