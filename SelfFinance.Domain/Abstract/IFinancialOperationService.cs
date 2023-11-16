using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Domain.Abstract;

public interface IFinancialOperationService
{
    public Task<Dictionary<OperationType, decimal>> CalculateTotalAsync(IEnumerable<FinancialOperationDto> operations);
    public Task<FinancialOperation> GetAsync(int id);
    public Task<IEnumerable<FinancialOperationDto>> GetAllAsync();
    public Task<IEnumerable<FinancialOperationDto>> GetAllAsync(DateTime from, DateTime to);
    public Task<int> AddAsync(FinancialOperationCreateDto dto);
    public Task UpdateAsync(FinancialOperationUpdateDto dto);
    public Task SoftDeleteAsync(int id);
}