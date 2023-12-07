using System.Collections.ObjectModel;
using System.Net;
using Microsoft.AspNetCore.Components;
using SelfFinance.Blazor.Components;
using SelfFinance.Blazor.Components.Abstract;
using SelfFinance.Client.Abstract;
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
    private WarningPopup _warningPopup;

    [Inject] private ITransactionService TransactionService { get; set; }
    [Inject] private IOperationTagService OperationTagService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _transactions = new ObservableCollection<TransactionViewModel>(await TransactionService.GetAllAsync());
            _operationTags = await OperationTagService.GetAllAsync();
        }
        catch (HttpRequestException)
        {
            _warningPopup.PopupAsync("Failed to load data", "The server is not accessible at the moment");
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
            _transactions!.Insert(0, addedTransaction);

            // ------ sort by date ------
            // var sortedTransactions = new List<TransactionViewModel>(_transactions!)
            //     .OrderByDescending(t => t.Date);
            //
            // _transactions!.Clear();
            //
            // foreach (var transaction in sortedTransactions)
            // {
            //     _transactions.Add(transaction);
            // }
            // ---------------------------
            _addModal.Hide();
        }
        catch (HttpRequestException ex)
        {
            _addModal.ErrorMessage = ex.StatusCode switch
            {
                HttpStatusCode.BadRequest => "Validation error occured",
                _ => "Some server error occured"
            };
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
        catch (HttpRequestException ex)
        {
            _editModal.ErrorMessage = ex.StatusCode switch
            {
                HttpStatusCode.BadRequest => "Validation error occured",
                _ => "Some server error occured"
            };
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
            catch (HttpRequestException ex)
            {
                _warningPopup.PopupAsync("Server error", ex.Message);
            }
        }
    }
}