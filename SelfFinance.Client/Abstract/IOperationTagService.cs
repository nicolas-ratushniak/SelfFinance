using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Client.Abstract;

public interface IOperationTagService
{
    public Task<OperationTagDto> GetAsync(int id);
    public Task<IEnumerable<OperationTagViewModel>> GetAllAsync();
    public Task<int> AddAsync(OperationTagCreateDto dto);
    public Task UpdateAsync(OperationTagUpdateDto dto);
    public Task SoftDeleteAsync(int id);
}