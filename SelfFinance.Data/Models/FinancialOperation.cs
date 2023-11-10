using System.ComponentModel.DataAnnotations.Schema;

namespace SelfFinance.Data.Models;

public class FinancialOperation
{
    public int Id { get; set; }
    public bool IsIncome { get; set; }
    [Column(TypeName = "numeric(9,2)")] public decimal Sum { get; set; }
    public int? IncomeTagId { get; set; }
    public IncomeTag? IncomeTag { get; set; }
    public int? ExpenseTagId { get; set; }
    public ExpenseTag? ExpenseTag { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
}