using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SelfFinance.Domain.Dto;

public class TransactionCreateDto
{
    [Required]
    [JsonProperty("sum")]
    [Range(1, 1_000_000, ErrorMessage = "Value must be between 1 and 1000000")]
    public decimal Sum { get; set; }

    [Required]
    [JsonProperty("operationDate")]
    public DateTime OperationDate { get; set; }

    [Required]
    [JsonProperty("operationTagId")]
    [Range(1, int.MaxValue, ErrorMessage = "None of tags was selected")]
    public int OperationTagId { get; set; }
}