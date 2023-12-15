using Newtonsoft.Json;

namespace SelfFinance.Domain.Dto;

public class ReportDto
{
    [JsonProperty("totalIncome")] public decimal TotalIncome { get; set; }
    [JsonProperty("totalExpense")] public decimal TotalExpense { get; set; }
    [JsonProperty("transactions")] public List<TransactionRichDto> Transactions { get; set; }
}