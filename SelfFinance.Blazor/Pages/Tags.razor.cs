﻿using System.Collections.ObjectModel;
using System.Net;
using Microsoft.AspNetCore.Components;
using SelfFinance.Blazor.Components;
using SelfFinance.Blazor.Components.Abstract;
using SelfFinance.Client.Abstract;
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
    private Popup _popup;
    
    [Inject] private IOperationTagService OperationTagService { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        
        try
        {
            _tags = new ObservableCollection<OperationTagViewModel>(await OperationTagService.GetAllAsync());
        }
        catch (HttpRequestException)
        {
            _popup.PopupAsync(PopupType.Error, "Failed to load data", 
                "The server is not accessible at the moment");
        }
    }

    // protected override async Task On()
    // {
    //     try
    //     {
    //         _tags = new ObservableCollection<OperationTagViewModel>(await OperationTagService.GetAllAsync());
    //     }
    //     catch (HttpRequestException)
    //     {
    //         _popup.PopupAsync(PopupType.Error, "Failed to load data", 
    //             "The server is not accessible at the moment");
    //     }
    // }

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
            _popup.PopupAsync(PopupType.Success, "New tag added");
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

    private async Task HandleUpdateAsync(OperationTagUpdateDto dto)
    {
        try
        {
            await OperationTagService.UpdateAsync(dto);

            var updatedTag = await OperationTagService.GetAsync(dto.Id);
            var oldTag = _tags!.Single(t => t.Id == dto.Id);

            oldTag.Name = updatedTag.Name;

            _editModal.Hide();
            _popup.PopupAsync(PopupType.Success, "The tag was updated");
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

    private async Task HandleDeleteModalResultAsync(ModalResult result)
    {
        var tagToDelete = _tagToDelete;
        _tagToDelete = null;

        if (result == ModalResult.Success)
        {
            try
            {
                await OperationTagService.SoftDeleteAsync(tagToDelete!.Id);
                _tags!.Remove(tagToDelete);
                _popup.PopupAsync(PopupType.Success, "The tag was deleted");
            }
            catch (HttpRequestException ex)
            {
                _popup.PopupAsync(PopupType.Error, "Server error", ex.Message);
            }
        }
    }
}