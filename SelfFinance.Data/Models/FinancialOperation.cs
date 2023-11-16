using System.ComponentModel.DataAnnotations.Schema;

namespace SelfFinance.Data.Models;

public class FinancialOperation
{
    public int Id { get; set; }
    [Column(TypeName = "numeric(9,2)")] public decimal Sum { get; set; }
    public int OperationTagId { get; set; }
    public OperationTag OperationTag { get; set; }
    public DateTime OperationDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
}