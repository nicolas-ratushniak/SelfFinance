using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Domain.Abstract;

public interface IExpenseTagService
{
    public Task<ExpenseTag> GetAsync(int id);
    public Task<IEnumerable<ExpenseTag>> GetAllAsync();
    public Task<int> AddAsync(ExpenseTagCreateDto dto);
    public Task UpdateAsync(ExpenseTagUpdateDto dto);
    public Task SoftDeleteAsync(int id);
}