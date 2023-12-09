using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SelfFinance.Domain.Dto;

public class OperationTagUpdateDto
{
    [Required]
    [JsonProperty("id")] 
    public int Id { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "Word from 2 to 30 symbols expected")]
    [JsonProperty("name")]
    public string Name { get; set; }
}