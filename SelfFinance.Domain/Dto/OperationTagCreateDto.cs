﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SelfFinance.Data.Models;

namespace SelfFinance.Domain.Dto;

public class OperationTagCreateDto
{
    [Required]
    [Range(1, 2, ErrorMessage = "Specify a correct transaction type")]
    [JsonProperty("operationType")]
    public OperationType OperationType { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "Word from 2 to 30 symbols expected")]
    [JsonProperty("name")]
    public string Name { get; set; }
}