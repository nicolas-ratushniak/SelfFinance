using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Domain.Abstract;

public interface ITransactionService
{
    public Task<Transaction> GetAsync(int id);
    public Task<IEnumerable<TransactionDto>> GetAllAsync();
    public Task<IEnumerable<TransactionDto>> GetAllAsync(DateTime from, DateTime to);
    public Task<int> AddAsync(TransactionCreateDto dto);
    public Task UpdateAsync(TransactionUpdateDto dto);
    public Task SoftDeleteAsync(int id);
}