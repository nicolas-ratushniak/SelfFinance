using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Api.Helpers;

public static class OperationTagHelper
{
    public static OperationTagDto ToDto(OperationTag tag)
    {
        return new OperationTagDto
        {
            Id = tag.Id,
            OperationType = tag.OperationType,
            Name = tag.Name
        };
    }
}