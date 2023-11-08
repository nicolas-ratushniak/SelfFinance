using System.ComponentModel.DataAnnotations;

namespace SelfFinance.Domain.Dto;

public class IncomeTagCreateDto
{
    [Required, StringLength(30, MinimumLength = 2)] public string Name { get; set; }
}