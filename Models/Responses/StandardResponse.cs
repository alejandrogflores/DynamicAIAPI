namespace DynamicAIAPI.Models.Responses;

public class StandardResponse
{
    public bool IsSuccess { get; set; }
    public object? Response { get; set; }
    public string? ErrorMessage { get; set; }
    public decimal DollarCost { get; set; }
}
