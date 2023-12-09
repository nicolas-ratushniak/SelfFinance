using SelfFinance.Client.ViewModels;
using SelfFinance.Data.Models;
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
            TagName = tagDto.Name,
            SignedSum = tagDto.OperationType switch
            {
                OperationType.Expense => -dto.Sum,
                OperationType.Income =>   dto.Sum,
                _ => throw new ArgumentException("Unsupportable operation type passed")
            }
        };
    }
    
    public static IEnumerable<TransactionViewModel> ConvertToViewModels(this IEnumerable<TransactionDto> dtos, IEnumerable<OperationTagDto> tagDtos)
    {
        return dtos.Select(t => t.ConvertToViewModel(
            tagDtos.Single(tag => tag.Id == t.OperationTagId)))
            .OrderByDescending(t => t.Date);
    }
}