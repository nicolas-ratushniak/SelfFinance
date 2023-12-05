using Newtonsoft.Json;

namespace SelfFinance.Domain.Dto;

public class TransactionDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("sum")] public decimal Sum { get; set; }
    [JsonProperty("operationDate")] public DateTime OperationDate { get; set; }
    [JsonProperty("operationTagId")] public int OperationTagId { get; set; }
}