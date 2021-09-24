using Newtonsoft.Json;
using System;

namespace Infra.Model
{
    public class TodoItem
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "assignedFor")]
        public string AssignedFor { get; set; }

        [JsonProperty(PropertyName = "status")]
        public State Status { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }


        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName ="pk")]
        public string PartitionKey { get; set; } = "todo";
    }

    public enum State
    {
        Backlog = 1,
        InProgress = 2,
        Done = 3
    }
}
