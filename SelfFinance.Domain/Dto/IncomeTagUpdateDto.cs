using System.ComponentModel.DataAnnotations;

namespace SelfFinance.Domain.Dto;

public class IncomeTagUpdateDto
{
    [Required] public int Id { get; set; }
    [Required, StringLength(30, MinimumLength = 2)] public string Name { get; set; }
}