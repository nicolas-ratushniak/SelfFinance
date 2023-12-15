using System.ComponentModel.DataAnnotations;

namespace SelfFinance.Data.Models;

public class OperationTag
{
    public int Id { get; set; }
    public OperationType OperationType { get; set; }
    [MaxLength(30)] public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}