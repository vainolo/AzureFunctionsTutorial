using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Vainolo.AzureFunctionsTutorial.Part5
{
    public class UserData
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Text { get; set; }
    }
    [StorageAccount("MyStorageAccount")]
    public static class SavingUserInput
    {
        [FunctionName("SavingUserInput")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Table("UserData")] out UserData ud,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string text = req.Query["text"];
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            text = text ?? data?.text;

            ud = new UserData
            {
                PartitionKey = "1",
                RowKey = DateTime.Now.Ticks.ToString(),
                Text = text
            };

            return text != null
                ? (ActionResult)new OkObjectResult($"Hello, {text}")
                : new BadRequestObjectResult("Please pass some text on the query string or in the request body");
        }
    }
}