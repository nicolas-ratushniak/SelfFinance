using System.Collections.ObjectModel;
using SelfFinance.Blazor.Components;
using SelfFinance.Blazor.Components.Abstract;
using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Blazor.Pages;

public partial class Tags
{
    private ObservableCollection<OperationTagViewModel>? _tags;
    private OperationTagViewModel? _tagToDelete;

    private ModalDelete _deleteModal;
    private TagCreateModal _addModal;
    private TagUpdateModal _editModal;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            _tags = new ObservableCollection<OperationTagViewModel>(await OperationTagService.GetAllAsync());
        }
        catch (HttpRequestException)
        {
            // handle
        }
    }

    private void ShowDeleteModal(OperationTagViewModel tag)
    {
        _tagToDelete = tag;
        _deleteModal.Show();
    }

    private void ShowEditModal(OperationTagViewModel tag)
    {
        var updateDto = new OperationTagUpdateDto
        {
            Id = tag.Id,
            Name = tag.Name
        };

        _editModal.Tag = updateDto;
        _editModal.Show();
    }

    private async Task HandleAddAsync(OperationTagCreateDto dto)
    {
        try
        {
            var id = await OperationTagService.AddAsync(dto);
            var addedTransaction = await OperationTagService.GetAsync(id);

            // to avoid refreshing all items from DB
            _tags!.Insert(0, addedTransaction);
            
            // ------ sort by name ------
            // var sortedTags = new List<OperationTagViewModel>(_tags!)
            //     .OrderByDescending(t => t.Name);
            //
            // _tags!.Clear();
            //
            // foreach (var tag in sortedTags)
            // {
            //     _tags.Add(tag);
            // }
            // ---------------------------
            
            _addModal.Hide();
        }
        catch (HttpRequestException)
        {
            _addModal.ErrorMessage = "Some server error occured";
        }
    }

    private async Task HandleUpdateAsync(OperationTagUpdateDto dto)
    {
        try
        {
            await OperationTagService.UpdateAsync(dto);

            var updatedTag = await OperationTagService.GetAsync(dto.Id);
            var oldTag = _tags!.Single(t => t.Id == dto.Id);

            // to avoid refreshing all items from DB
            oldTag.Name = updatedTag.Name;

            _editModal.Hide();
        }
        catch (HttpRequestException)
        {
            _editModal.ErrorMessage = "Some server error occured";
        }
    }

    private async Task HandleDeleteModalResultAsync(ModalResult result)
    {
        var tagToDelete = _tagToDelete;
        _tagToDelete = null;

        if (result == ModalResult.Success)
        {
            try
            {
                await OperationTagService.SoftDeleteAsync(tagToDelete!.Id);
                // to avoid refreshing all items from DB
                _tags!.Remove(tagToDelete);
            }
            catch (HttpRequestException)
            {
                // handle
            }
        }
    }
}