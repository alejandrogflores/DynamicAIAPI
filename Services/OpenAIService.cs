using OpenAI.Chat;
using DynamicAIAPI.Models.Responses;

namespace DynamicAIAPI.Services;

public class OpenAIService
{
    private readonly ChatClient _chatClient;

    public OpenAIService(ChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<StandardResponse> ExecuteChatAsync(string prompt)
    {
        try
        {
            // Llamada simple al modelo de chat con un solo string
            ChatCompletion completion = await _chatClient.CompleteChatAsync(prompt);

            var text = completion.Content[0].Text;

            return new StandardResponse
            {
                IsSuccess = true,
                Response = text,
                DollarCost = 0m // si luego quieres calculamos costo real
            };
        }
        catch (Exception ex)
        {
            return new StandardResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message,
                DollarCost = 0m
            };
        }
    }
}





