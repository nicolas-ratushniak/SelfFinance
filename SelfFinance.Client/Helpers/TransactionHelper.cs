using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Client.Helpers;

public static class TransactionHelper
{
    public static TransactionViewModel ConvertToViewModel(this TransactionDto dto, OperationTagDto tagDto)
    {
        if (tagDto is null)
        {
            throw new ArgumentNullException(nameof(tagDto));
        }
        
        return new TransactionViewModel
        {
            Id = dto.Id,
            Date = dto.OperationDate,
            TagId = dto.OperationTagId,
            TagName = tagDto.Name,
            Type = tagDto.OperationType,
            AbsoluteSum = dto.Sum
        };
    }
}