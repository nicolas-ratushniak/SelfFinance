using SelfFinance.Client.ViewModels;
using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Client.Helpers;

public static class TransactionHelper
{
    public static TransactionViewModel ConvertToViewModel(this TransactionRichDto dto)
    {
        
        return new TransactionViewModel
        {
            Id = dto.Id,
            Date = dto.OperationDate,
            TagName = dto.OperationTag.Name,
            SignedSum = dto.OperationTag.OperationType switch
            {
                OperationType.Expense => -dto.Sum,
                OperationType.Income =>   dto.Sum,
                _ => throw new ArgumentException("Unsupportable operation type passed")
            }
        };
    }
}