using SelfFinance.Data.Models;

namespace SelfFinance.ApiConsumer.ViewModels;

public class OperationTagViewModel
{
    public int Id { get; set; }
    public OperationType OperationType { get; set; }
    public string Name { get; set; }
}