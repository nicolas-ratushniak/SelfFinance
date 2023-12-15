using SelfFinance.Client.ViewModels;
using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Client.Helpers;

public static class OperationTagHelper
{
    public static OperationTagViewModel ConvertToViewModel(this OperationTagDto dto)
    {
        return new OperationTagViewModel
        {
            Id = dto.Id,
            Type = dto.OperationType switch
            {
                OperationType.Income => "Income",
                OperationType.Expense => "Expense",
                _ => throw new ArgumentException("Unsupportable operation type passed")
            },
            Name = dto.Name
        };
    }
}