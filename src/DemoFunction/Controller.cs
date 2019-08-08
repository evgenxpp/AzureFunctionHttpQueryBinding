namespace DemoFunction
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Azure.WebJobs.HttpTriggerQueryBinding;
    using System.Collections.Generic;

    public class Controller
    {
        [FunctionName(nameof(DemoFunction))]
        public async Task<IActionResult> DemoFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Query("name")] string name,
            [Query] IEnumerable<string> childName,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (string.IsNullOrWhiteSpace(name))
            {
                return new BadRequestObjectResult(
                    "Please pass a 'name' on the query string and optional child names like 'childName=A&childName=B'");
            }

            using (var streamReader = new StreamReader(req.Body))
            {
                var formattedChildNames = childName != null
                    ? string.Join(" | ", childName)
                    : "none";

                var requestBody = await streamReader.ReadToEndAsync();
                return new OkObjectResult($"Hello, {name}\nChild names: {formattedChildNames}");
            }
        }
    }
}
