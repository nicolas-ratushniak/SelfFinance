using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SelfFinance.Domain.Dto;

public class TransactionCreateDto
{
    [Required, Range(1, 1_000_000), JsonProperty("sum")]
    public decimal Sum { get; set; }

    [Required, JsonProperty("operationDate")]
    public DateTime OperationDate { get; set; }

    [Required, JsonProperty("operationTagId")]
    public int OperationTagId { get; set; }
}