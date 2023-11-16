using System.ComponentModel.DataAnnotations;
using SelfFinance.Data.Models;

namespace SelfFinance.Domain.Dto;

public class OperationTagCreateDto
{
    [Required] public OperationType OperationType { get; set; }
    [Required, StringLength(30, MinimumLength = 2)] public string Name { get; set; }
}