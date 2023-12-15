using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SelfFinance.Domain.Dto;

public class TransactionUpdateDto
{
    [Required]
    [JsonProperty("id")] 
    public int Id { get; set; }

    [Required]
    [Range(1, 1_000_000, ErrorMessage = "Enter sum from $1 to $1,000,000")]
    [JsonProperty("sum")]
    public decimal Sum { get; set; }

    [Required]
    [JsonProperty("operationDate")]
    public DateTime OperationDate { get; set; }

    [Required]
    [JsonProperty("operationTagId")]
    [Range(1, int.MaxValue, ErrorMessage = "None of tags was selected")]
    public int OperationTagId { get; set; }
}