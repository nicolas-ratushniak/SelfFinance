using SelfFinance.Data.Models;

namespace SelfFinance.Client.ViewModels;

public class TransactionViewModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int TagId { get; set; }
    public string TagName { get; set; }
    public OperationType Type { get; set; }
    public decimal AbsoluteSum { get; set; }
}