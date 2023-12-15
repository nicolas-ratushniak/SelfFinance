using Newtonsoft.Json;
using SelfFinance.Data.Models;

namespace SelfFinance.Domain.Dto;

public class OperationTagDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("operationType")] public OperationType OperationType { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
}