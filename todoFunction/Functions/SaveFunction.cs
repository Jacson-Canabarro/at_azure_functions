using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Infra.Model;
using Infra.Repository;

namespace todoFunction.Functions
{
    public static class SaveFunction
    {
        [FunctionName("Save")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            TodoItem data = JsonConvert.DeserializeObject<TodoItem>(requestBody);

            if (data == null)
                return new BadRequestObjectResult(new { message = "Dados da tarefa são obrigatorios" });


            var repository = new TodoRepository();

            data.Id = Guid.NewGuid().ToString();

            await repository.Save(data);

            return new CreatedResult("", data);
        }
    }
}
