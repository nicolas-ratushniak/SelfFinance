﻿using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Client.Abstract;

public interface ITransactionService
{
    public Task<TransactionRichDto> GetAsync(int id);
    public Task<IEnumerable<TransactionViewModel>> GetAllAsync();
    public Task<int> AddAsync(TransactionCreateDto dto);
    public Task UpdateAsync(TransactionUpdateDto dto);
    public Task SoftDeleteAsync(int id);
}