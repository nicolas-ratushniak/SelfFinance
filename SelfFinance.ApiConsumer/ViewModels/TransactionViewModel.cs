using SelfFinance.Data.Models;

namespace SelfFinance.ApiConsumer.ViewModels;

public class TransactionViewModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Tag { get; set; }
    public OperationType Type { get; set; }
    public decimal AbsoluteSum { get; set; }
}