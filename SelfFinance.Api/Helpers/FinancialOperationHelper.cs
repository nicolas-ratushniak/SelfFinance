using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Api.Helpers;

public static class FinancialOperationHelper
{
    public static FinancialOperationDto OperationToDto(FinancialOperation operation)
    {
        return new FinancialOperationDto
        {
            Id = operation.Id,
            Sum = operation.Sum,
            OperationTagId = operation.OperationTagId
        };
    }
}