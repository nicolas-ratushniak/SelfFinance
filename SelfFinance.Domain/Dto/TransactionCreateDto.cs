﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SelfFinance.Domain.Dto;

public class TransactionCreateDto
{
    [Required]
    [JsonProperty("sum")]
    [Range(1, 1_000_000, ErrorMessage = "Enter sum from $1 to $1,000,000")]
    public decimal Sum { get; set; }

    [Required]
    [JsonProperty("operationDate")]
    public DateTime OperationDate { get; set; }

    [Required]
    [JsonProperty("operationTagId")]
    [Range(1, int.MaxValue, ErrorMessage = "None of tags was selected")]
    public int OperationTagId { get; set; }
}