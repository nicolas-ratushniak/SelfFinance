using System.Text;
using Newtonsoft.Json;
using SelfFinance.Client.Abstract;
using SelfFinance.Client.Helpers;
using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Client.Services;

public class TransactionService : ITransactionService
{
    private readonly HttpClient _client;

    public TransactionService(HttpClient client)
    {
        _client = client;
    }

    public async Task<TransactionViewModel> GetAsync(int id)
    {
        using var transactionResponse = await _client.GetAsync($"transactions/{id}");
        transactionResponse.EnsureSuccessStatusCode();

        var transactionJsonResult = await transactionResponse.Content.ReadAsStringAsync();
        var transactionDto = JsonConvert.DeserializeObject<TransactionDto>(transactionJsonResult)!;

        using var tagResponse = await _client.GetAsync($"tags/{transactionDto.OperationTagId}");
        tagResponse.EnsureSuccessStatusCode();

        var tagJsonResult = await tagResponse.Content.ReadAsStringAsync();
        var tagDto = JsonConvert.DeserializeObject<OperationTagDto>(tagJsonResult)!;

        return transactionDto.ConvertToViewModel(tagDto);
    }

    public async Task<IEnumerable<TransactionViewModel>> GetAllAsync()
    {
        using var transactionsResponse = await _client.GetAsync("transactions");
        using var tagsResponse = await _client.GetAsync("tags/include-deleted");

        transactionsResponse.EnsureSuccessStatusCode();
        tagsResponse.EnsureSuccessStatusCode();

        var transactionsJsonResult = await transactionsResponse.Content.ReadAsStringAsync();
        var transactionDtos = JsonConvert.DeserializeObject<IEnumerable<TransactionDto>>(transactionsJsonResult)!;

        var tagsJsonResult = await tagsResponse.Content.ReadAsStringAsync();
        var tagDtos = JsonConvert.DeserializeObject<IEnumerable<OperationTagDto>>(tagsJsonResult)!;

        return transactionDtos
            .Select(t => t.ConvertToViewModel(
                tagDtos.Single(tag => tag.Id == t.OperationTagId)))
            .OrderByDescending(t => t.Date);
    }

    public async Task<int> AddAsync(TransactionCreateDto dto)
    {
        var body = JsonConvert.SerializeObject(dto);

        using var jsonContent = new StringContent(body, Encoding.UTF8, "application/json");
        using var response = await _client.PostAsync("transactions", jsonContent);
        
        response.EnsureSuccessStatusCode();

        var jsonResult = await response.Content.ReadAsStringAsync();
        var id = JsonConvert.DeserializeObject<int>(jsonResult);

        return id;
    }

    public async Task UpdateAsync(TransactionUpdateDto dto)
    {
        var body = JsonConvert.SerializeObject(dto);

        using var jsonContent = new StringContent(body, Encoding.UTF8, "application/json");
        using var response = await _client.PutAsync("transactions", jsonContent);

        response.EnsureSuccessStatusCode();
    }

    public async Task SoftDeleteAsync(int id)
    {
        using var response = await _client.DeleteAsync($"transactions/{id}");
        response.EnsureSuccessStatusCode();
    }
}