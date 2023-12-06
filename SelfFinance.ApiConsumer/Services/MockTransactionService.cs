using SelfFinance.ApiConsumer.Abstract;
using SelfFinance.ApiConsumer.ViewModels;
using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.ApiConsumer.Services;

public class MockTransactionService : ITransactionService
{
    private static int _currentId = 3;
    
    private readonly List<TransactionViewModel> _items = new()
    {
        new()
        {
            Id = 1, Date = new DateTime(2023, 12, 4), Tag = "Salary", Type = OperationType.Income, AbsoluteSum = 4000
        },
        new()
        {
            Id = 2, Date = new DateTime(2023, 12, 3), Tag = "Food", Type = OperationType.Expense, AbsoluteSum = 20
        },
        new()
        {
            Id = 3, Date = new DateTime(2023, 12, 3), Tag = "Taxi", Type = OperationType.Expense, AbsoluteSum = 45.50M
        }
    };

    public async Task<TransactionViewModel> GetAsync(int id)
    {
        return _items.Single(i => i.Id == id);
    }

    public async Task<IEnumerable<TransactionViewModel>> GetAllAsync()
    {
        return _items;
    }

    public async Task<int> AddAsync(TransactionCreateDto dto)
    {
        _items.Add(new TransactionViewModel
        {
            Id = ++_currentId,
            Date = dto.OperationDate,
            AbsoluteSum = dto.Sum
        });

        return _currentId;
    }

    public async Task UpdateAsync(TransactionUpdateDto dto)
    {
        var item = await GetAsync(dto.Id);

        item.Date = dto.OperationDate;
        item.AbsoluteSum = dto.Sum;
        // item.Tag = dto.OperationTagId;
    }

    public async Task SoftDeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _items.Remove(item);
    }
}