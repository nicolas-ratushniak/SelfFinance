using Newtonsoft.Json;

namespace SelfFinance.Domain.Dto;

public class TransactionRichDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("sum")] public decimal Sum { get; set; }
    [JsonProperty("operationDate")] public DateTime OperationDate { get; set; }
    [JsonProperty("operationTag")] public OperationTagDto OperationTag { get; set; }
}