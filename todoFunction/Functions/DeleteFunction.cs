using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Infra.Repository;
using Infra.Model;

namespace todoFunction.Functions
{
    public static class DeleteFunction
    {
        [FunctionName("Delete")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            TodoItem data = JsonConvert.DeserializeObject<TodoItem>(requestBody);

            if (data == null || data.Id == null)
                return new BadRequestObjectResult(new { message = "Dados da tarefa são obrigatórios" });

            var repository = new TodoRepository();

            var todo = repository.Delete(data);

            return new OkObjectResult(new { message = "Tarefa deletada com sucesso."});
        }
    }
}
