using SelfFinance.ApiConsumer.Abstract;
using SelfFinance.ApiConsumer.ViewModels;
using SelfFinance.Data.Models;
using SelfFinance.Domain.Dto;

namespace SelfFinance.ApiConsumer.Services;

public class MockOperationTagService : IOperationTagService
{
    private static int _currentId = 3;

    private readonly List<OperationTagViewModel> _items = new()
    {
        new OperationTagViewModel { Id = 1, OperationType = OperationType.Income, Name = "Salary" },
        new OperationTagViewModel { Id = 2, OperationType = OperationType.Expense, Name = "Food" },
        new OperationTagViewModel { Id = 3, OperationType = OperationType.Expense, Name = "Taxi" }
    };

    public async Task<OperationTagViewModel> GetAsync(int id)
    {
        return _items.Single(i => i.Id == id);
    }

    public async Task<IEnumerable<OperationTagViewModel>> GetAllAsync()
    {
        return _items;
    }

    public async Task<int> AddAsync(OperationTagCreateDto dto)
    {
        _items.Add(new OperationTagViewModel
        {
            Id = ++_currentId,
            OperationType = dto.OperationType,
            Name = dto.Name
        });

        return _currentId;
    }

    public async Task UpdateAsync(OperationTagUpdateDto dto)
    {
        var item = await GetAsync(dto.Id);

        item.Name = dto.Name;
    }

    public async Task SoftDeleteAsync(int id)
    {
        var item = await GetAsync(id);
        _items.Remove(item);
    }
}