using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Api.Helpers;

public static class TransactionHelper
{
    public static TransactionDto ConvertToDto(this Transaction operation)
    {
        return new TransactionDto
        {
            Id = operation.Id,
            Sum = operation.Sum,
            OperationDate = operation.OperationDate,
            OperationTagId = operation.OperationTagId
        };
    }
}