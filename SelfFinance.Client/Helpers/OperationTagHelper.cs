using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Client.Helpers;

public static class OperationTagHelper
{
    public static OperationTagViewModel ConvertToViewModel(this OperationTagDto dto)
    {
        return new OperationTagViewModel
        {
            Id = dto.Id,
            OperationType = dto.OperationType,
            Name = dto.Name
        };
    }
}