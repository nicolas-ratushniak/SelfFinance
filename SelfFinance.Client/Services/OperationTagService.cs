using System.Text;
using Newtonsoft.Json;
using SelfFinance.Client.Abstract;
using SelfFinance.Client.Helpers;
using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Client.Services;

public class OperationTagService : IOperationTagService
{
    private readonly HttpClient _client;

    public OperationTagService(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<OperationTagViewModel> GetAsync(int id)
    {
        using var response = await _client.GetAsync($"tags/{id}");
        response.EnsureSuccessStatusCode();

        var jsonResult = await response.Content.ReadAsStringAsync();
        var tagDto = JsonConvert.DeserializeObject<OperationTagDto>(jsonResult);

        return tagDto!.ConvertToViewModel();
    }

    public async Task<IEnumerable<OperationTagViewModel>> GetAllAsync()
    {
        using var response = await _client.GetAsync("tags");
        response.EnsureSuccessStatusCode();

        var jsonResult = await response.Content.ReadAsStringAsync();
        var tagDtos = JsonConvert.DeserializeObject<IEnumerable<OperationTagDto>>(jsonResult);

        return tagDtos!.Select(dto => dto.ConvertToViewModel());
    }

    public async Task<int> AddAsync(OperationTagCreateDto dto)
    {
        var body = JsonConvert.SerializeObject(dto);

        using var jsonContent = new StringContent(body, Encoding.UTF8, "application/json");
        using var response = await _client.PostAsync("tags", jsonContent);
        
        response.EnsureSuccessStatusCode();

        var jsonResult = await response.Content.ReadAsStringAsync();
        var id = JsonConvert.DeserializeObject<int>(jsonResult);

        return id;
    }

    public async Task UpdateAsync(OperationTagUpdateDto dto)
    {
        var body = JsonConvert.SerializeObject(dto);

        using var jsonContent = new StringContent(body, Encoding.UTF8, "application/json");
        using var response = await _client.PutAsync("tags", jsonContent);
        
        response.EnsureSuccessStatusCode();
    }

    public async Task SoftDeleteAsync(int id)
    {
        using var response = await _client.DeleteAsync($"tags/{id}");
        response.EnsureSuccessStatusCode();
    }
}