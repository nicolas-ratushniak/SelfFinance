using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Domain.Abstract;

public interface IIncomeTagService
{
    public Task<IncomeTag> GetAsync(int id);
    public Task<IEnumerable<IncomeTag>> GetAllAsync();
    public Task<int> AddAsync(IncomeTagCreateDto dto);
    public Task UpdateAsync(IncomeTagUpdateDto dto);
    public Task SoftDeleteAsync(int id);
}