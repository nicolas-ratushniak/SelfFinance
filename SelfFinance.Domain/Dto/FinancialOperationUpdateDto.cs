using System.ComponentModel.DataAnnotations;

namespace SelfFinance.Domain.Dto;

public class FinancialOperationUpdateDto
{
    [Required] public int Id { get; set; }
    public int? TagId { get; set; }
}