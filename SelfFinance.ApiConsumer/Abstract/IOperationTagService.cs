using SelfFinance.ApiConsumer.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.ApiConsumer.Abstract;

public interface IOperationTagService
{
    public Task<OperationTagViewModel> GetAsync(int id);
    public Task<IEnumerable<OperationTagViewModel>> GetAllAsync();
    public Task<int> AddAsync(OperationTagCreateDto dto);
    public Task UpdateAsync(OperationTagUpdateDto dto);
    public Task SoftDeleteAsync(int id);
}