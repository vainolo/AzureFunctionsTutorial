using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Microsoft.Azure.WebJobs.Extensions.Timers;        

namespace Vainolo.AzureFunctionsTutorial.Part6
{
    public static class MyFunctions
    {
        [FunctionName("HttpTrigger")]
        public static async Task<IActionResult> HttpTrigger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }


        [FunctionName("NoTrigger")]
        public static void NoTrigger(ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
        }        

        // CRON formatted trigger every 5 seconds
        [FunctionName("TimerTrigger1")]
        public static void TimerTrigger1([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"CRON trigger executed at : {DateTime.Now}");
        }

        [FunctionName("TimerTrigger2")]
        public static void TimerTrigger2([TimerTrigger("00:00:03")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"TimeSpan trigger executed at : {DateTime.Now}");
        }
    }
}
