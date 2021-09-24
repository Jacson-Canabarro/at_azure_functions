using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Infra.Repository;

namespace todoFunction
{
    public static class GetByIdFunction
    {
        [FunctionName("GetById")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string qId = req.Query["id"];
            if(qId == null || qId == "")
                return new BadRequestObjectResult(new { message = "O ID � obrigat�rio para esta opera��o." });
            
            Guid id = new Guid(qId);

            var repository = new TodoRepository();

            var todo = repository.GetById(id.ToString());
            if (todo == null)
                return new NotFoundObjectResult(new { message = "Tarefa n�o encontrada" });

            return new OkObjectResult(todo);
        }
    }
}
