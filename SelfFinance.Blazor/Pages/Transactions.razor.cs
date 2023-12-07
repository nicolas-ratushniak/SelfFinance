using System.Collections.ObjectModel;
using SelfFinance.Blazor.Components;
using SelfFinance.Blazor.Components.Abstract;
using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Blazor.Pages;

public partial class Transactions
{
    private ObservableCollection<TransactionViewModel>? _transactions;
    private IEnumerable<OperationTagViewModel>? _operationTags = new List<OperationTagViewModel>();
    private TransactionViewModel? _transactionToDelete;

    private ModalDelete _deleteModal;
    private TransactionCreateModal _addModal;
    private TransactionUpdateModal _editModal;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _transactions = new ObservableCollection<TransactionViewModel>(await TransactionService.GetAllAsync());
            _operationTags = await OperationTagService.GetAllAsync();
        }
        catch (HttpRequestException)
        {
            // handle
        }
    }

    private void ShowDeleteModal(TransactionViewModel transaction)
    {
        _transactionToDelete = transaction;
        _deleteModal.Show();
    }

    private void ShowEditModal(TransactionViewModel transaction)
    {
        var updateDto = new TransactionUpdateDto
        {
            Id = transaction.Id,
            Sum = transaction.AbsoluteSum,
            OperationDate = transaction.Date,
            OperationTagId = transaction.TagId
        };

        _editModal.Transaction = updateDto;
        _editModal.Show();
    }

    private async Task HandleAddAsync(TransactionCreateDto dto)
    {
        try
        {
            var id = await TransactionService.AddAsync(dto);
            var addedTransaction = await TransactionService.GetAsync(id);

            // to avoid refreshing all items from DB
            _transactions!.Add(addedTransaction);
            _addModal.Hide();
        }
        catch (HttpRequestException)
        {
            _addModal.ErrorMessage = "Some server error occured";
        }
    }

    private async Task HandleUpdateAsync(TransactionUpdateDto dto)
    {
        try
        {
            await TransactionService.UpdateAsync(dto);

            var updatedTransaction = await TransactionService.GetAsync(dto.Id);
            var oldTransaction = _transactions!.Single(t => t.Id == dto.Id);

            // to avoid refreshing all items from DB
            oldTransaction.Date = updatedTransaction.Date;
            oldTransaction.AbsoluteSum = updatedTransaction.AbsoluteSum;
            oldTransaction.Type = updatedTransaction.Type;
            oldTransaction.TagId = updatedTransaction.TagId;
            oldTransaction.TagName = updatedTransaction.TagName;

            _editModal.Hide();
        }
        catch (HttpRequestException)
        {
            _editModal.ErrorMessage = "Some server error occured";
        }
    }

    private async Task HandleDeleteModalResultAsync(ModalResult result)
    {
        var transactionToDelete = _transactionToDelete;
        _transactionToDelete = null;

        if (result == ModalResult.Success)
        {
            try
            {
                await TransactionService.SoftDeleteAsync(transactionToDelete!.Id);
                // to avoid refreshing all items from DB
                _transactions!.Remove(transactionToDelete);
            }
            catch (HttpRequestException)
            {
                // handle
            }
        }
    }
}