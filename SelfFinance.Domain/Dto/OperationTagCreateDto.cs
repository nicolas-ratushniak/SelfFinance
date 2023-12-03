using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SelfFinance.Data.Models;

namespace SelfFinance.Domain.Dto;

public class OperationTagCreateDto
{
    [Required, JsonProperty("operationType")]
    public OperationType OperationType { get; set; }

    [Required, StringLength(30, MinimumLength = 2), JsonProperty("name")]
    public string Name { get; set; }
}