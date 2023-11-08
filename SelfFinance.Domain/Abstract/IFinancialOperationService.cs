using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Domain.Abstract;

public interface IFinancialOperationService
{
    public (decimal totalIncomes, decimal totalExpenses) CalculateTotal(IEnumerable<FinancialOperation> operations);
    public Task<FinancialOperation> GetAsync(int id);
    public Task<IEnumerable<FinancialOperation>> GetAllAsync();
    public Task<IEnumerable<FinancialOperation>> GetAllAsync(DateTime from, DateTime to);
    public Task<int> AddAsync(FinancialOperationCreateDto dto);
    public Task UpdateAsync(FinancialOperationUpdateDto dto);
    public Task SoftDeleteAsync(int id);
}