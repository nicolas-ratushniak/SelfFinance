using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SelfFinance.Domain.Dto;

public class TransactionUpdateDto
{
    [Required, JsonProperty("id")] public int Id { get; set; }

    [Required, Range(1, 1_000_000), JsonProperty("sum")]
    public decimal Sum { get; set; }

    [Required, JsonProperty("operationDate")]
    public DateTime OperationDate { get; set; }

    [Required, JsonProperty("operationTagId")]
    public int OperationTagId { get; set; }
}