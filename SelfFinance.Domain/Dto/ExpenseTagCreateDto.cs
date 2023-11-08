using System.ComponentModel.DataAnnotations;

namespace SelfFinance.Domain.Dto;

public class ExpenseTagCreateDto
{
    [Required, StringLength(30, MinimumLength = 2)] public string Name { get; set; }
}