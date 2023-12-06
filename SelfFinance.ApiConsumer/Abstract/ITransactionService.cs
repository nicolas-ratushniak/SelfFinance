using SelfFinance.ApiConsumer.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.ApiConsumer.Abstract;

public interface ITransactionService
{
    public Task<TransactionViewModel> GetAsync(int id);
    public Task<IEnumerable<TransactionViewModel>> GetAllAsync();
    public Task<int> AddAsync(TransactionCreateDto dto);
    public Task UpdateAsync(TransactionUpdateDto dto);
    public Task SoftDeleteAsync(int id);
}