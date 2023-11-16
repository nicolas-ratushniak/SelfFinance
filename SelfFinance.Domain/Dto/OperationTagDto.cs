using SelfFinance.Data.Models;

namespace SelfFinance.Domain.Dto;

public class OperationTagDto
{
    public int Id { get; set; }
    public OperationType OperationType { get; set; }
    public string Name { get; set; }
}