using System.ComponentModel.DataAnnotations;

namespace SelfFinance.Data.Models;

public class ExpenseTag
{
    public int Id { get; set; }
    [MaxLength(30)] public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime DeletedOn { get; set; }
}