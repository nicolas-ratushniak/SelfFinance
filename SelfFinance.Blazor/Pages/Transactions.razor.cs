using System.Collections.ObjectModel;
using System.Net;
using Microsoft.AspNetCore.Components;
using SelfFinance.Blazor.Components.Modals;
using SelfFinance.Blazor.Components.Abstract;
using SelfFinance.Client.Abstract;
using SelfFinance.Client.Helpers;
using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Blazor.Pages;

public partial class Transactions
{
    private ObservableCollection<TransactionViewModel>? _transactions;
    private IEnumerable<OperationTagViewModel>? _operationTags = new List<OperationTagViewModel>();

    private ModalDelete<int> _deleteModal;
    private TransactionCreateModal _addModal;
    private TransactionUpdateModal _editModal;
    private Popup _popup;

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
            _popup.PopupAsync(PopupType.Error, "Failed to load data", "The server is not accessible at the moment");
        }
    }

    private void ShowDeleteModal(int id)
    {
        _deleteModal.Show(id);
    }

    private async Task ShowEditModalAsync(int id)
    {
        var transactionToEdit = await TransactionService.GetAsync(id);
        
        var updateDto = new TransactionUpdateDto
        {
            Id = transactionToEdit.Id,
            Sum = transactionToEdit.Sum,
            OperationDate = transactionToEdit.OperationDate,
            OperationTagId = transactionToEdit.OperationTag.Id
        };

        _editModal.Show(updateDto);
    }

    private async Task HandleAddAsync(TransactionCreateDto dto)
    {
        try
        {
            var id = await TransactionService.AddAsync(dto);
            var addedTransaction = await TransactionService.GetAsync(id);

            _transactions!.Insert(0, addedTransaction.ConvertToViewModel());
            
            _addModal.Hide();
            _popup.PopupAsync(PopupType.Success, "New transaction added");
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                _popup.PopupAsync(PopupType.Warning, "Oops!", "Validation error occured");
            }
            else
            {
                _popup.PopupAsync(PopupType.Error, "Oops!", "Some server error occured");
            }
        }
    }

    private async Task HandleUpdateAsync(TransactionUpdateDto dto)
    {
        try
        {
            await TransactionService.UpdateAsync(dto);

            var updatedTransaction = (await TransactionService.GetAsync(dto.Id)).ConvertToViewModel();
            var oldTransaction = _transactions!.Single(t => t.Id == dto.Id);

            oldTransaction.Date = updatedTransaction.Date;
            oldTransaction.TagName = updatedTransaction.TagName;
            oldTransaction.SignedSum = updatedTransaction.SignedSum;

            _editModal.Hide();
            _popup.PopupAsync(PopupType.Success, "The transaction was updated");
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                _popup.PopupAsync(PopupType.Warning, "Oops!", "Validation error occured");
            }
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _popup.PopupAsync(PopupType.Warning, "Oops!", "Tag not found. Maybe it was deleted");
            }
            else
            {
                _popup.PopupAsync(PopupType.Error, "Oops!", "Some server error occured");
            }
        }
    }

    private async Task DeleteAsync(int id)
    {
        try
        {
            await TransactionService.SoftDeleteAsync(id);
            _transactions!.Remove(_transactions.Single(t => t.Id == id));
            _popup.PopupAsync(PopupType.Success, "The transaction was deleted");
        }
        catch (HttpRequestException ex)
        {
            _popup.PopupAsync(PopupType.Error, "Server error", ex.Message);
        }
    }
}