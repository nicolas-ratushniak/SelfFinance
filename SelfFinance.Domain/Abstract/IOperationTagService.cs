using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Domain.Abstract;

public interface IOperationTagService
{
    public Task<OperationTag> GetAsync(int id);
    public Task<IEnumerable<OperationTagDto>> GetAllAsync();
    public Task<IEnumerable<OperationTagDto>> GetAllIncludeSoftDeletedAsync();
    public Task<int> AddAsync(OperationTagCreateDto dto);
    public Task UpdateAsync(OperationTagUpdateDto dto);
    public Task SoftDeleteAsync(int id);
}